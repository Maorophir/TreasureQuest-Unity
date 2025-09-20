using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement_1: MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time can the player hang in the air before jumping
    private float coyoteCounter; // Counts the time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; // Horizontal
    [SerializeField] private float wallJumpY; // Vertical

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    public bool cantMove;

    private float Xinput;
    [SerializeField] private GameObject pauseScreen;
    public static bool isCrouching;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!cantMove)
        {
            Xinput = Input.GetAxis("Horizontal");

            // Handles the flipping of the character
            if (Xinput > 0.01f)
                transform.localScale = Vector3.one;
            else if (Xinput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);

            // Set animator parameters
            animator.SetBool("Walk", Xinput != 0);
            animator.SetBool("Grounded", isGrounded());
                

            // Handle movement and jumping
            HandleMovement();
            HandleJump();
            Crouch();
            if (!isGrounded())
                coyoteCounter -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if (isGrounded())
        {
            // Allow normal movement on the ground
            body.velocity = new Vector2(Xinput * speed, body.velocity.y);
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else
        {
            if (!IsCollidingHorizontally())
            {
                // Apply horizontal movement only if not colliding with a horizontal object
                body.velocity = new Vector2(Xinput * speed, body.velocity.y);
            }
        }

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 1;
        }
    }

    private void HandleJump()
    {
        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && !pauseScreen.activeInHierarchy)
        {
            Jump();
        }

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }
    }

    private void Crouch()
    {
        if (isGrounded())
        {
            isCrouching = Input.GetKey(KeyCode.C);
            animator.SetBool("isCrouching", isCrouching);
            if (isCrouching)
                body.velocity = new Vector2(0, body.velocity.y); 
        }
    }

    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;

        if (isGrounded() || onWall())
            SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
        {
            WallJump();
        }
        else
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
            {
                if (coyoteCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                }
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
    }

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
        Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
        new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private bool IsCollidingHorizontally()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
        new Vector2(transform.localScale.x, 0), 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool canCast()
    {
        return Xinput == 0 && isGrounded() && !onWall();
    }

    public void CantMove()
    {
        cantMove = true;
        body.velocity = Vector2.zero;
    }

    public void CanMove()
    {
        cantMove = false;
    }
}
