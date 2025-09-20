using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header ("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;

    [Header ("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time can the player hang in the air before jumping
    private float coyoteCounter; // Counts the time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header ("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal
    [SerializeField] private float wallJumpY; // Vertical
    // private float wallJumpCooldown;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private float Xinput;
    [SerializeField] private GameObject pauseScreen;

 
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {   Xinput = Input.GetAxis("Horizontal");

        // Handles the flipping of the character
        if (Xinput > 0.01f)
            transform.localScale = Vector3.one;
        else if (Xinput < - 0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        //Set animator paramaters
        animator.SetBool("Walk", Xinput != 0);
        animator.SetBool("Grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !pauseScreen.activeInHierarchy)
            Jump();
        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 1;
            body.velocity = new Vector2(Xinput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote jump
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            }
            else
                coyoteCounter -= Time.deltaTime; // Start decreasing coyote counter when not on the ground
        }
    }

    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                // If not on the ground and coyote counter is bigger than 0 jump normally
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            //Reset coyoter counter to avoide double jumping
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
       body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
    //    wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
        Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
        new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canCast()
    {
        return Xinput == 0 && isGrounded() && !onWall();
    }
}
