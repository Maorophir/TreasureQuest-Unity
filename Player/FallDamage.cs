using UnityEngine;

public class FallDamage : MonoBehaviour
{
    [SerializeField] private float minFallHeight = 5f;
    [SerializeField] private float maxFallHeight = 10f;
    // [SerializeField] private float highFallDamage = 3f;
    [SerializeField] private float lowFallDamage = 1f;

    private float startY;
    private bool isFalling;
    private Health playerHealth;
    public PlayerMovement_1 playerMovement;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement_1>();
    }

    private void Update()
    {
        if (playerMovement.isGrounded())
        {
            if (isFalling)
            {
                float fallDistance = startY - transform.position.y;

                if (fallDistance >= maxFallHeight)
                {
                    // Die
                    playerHealth.TakeDamage(playerHealth.currentHealth);
                }
                else if (fallDistance >= minFallHeight)
                {
                    playerHealth.TakeDamage(lowFallDamage);
                }

                isFalling = false;
            }
        }
        else if (playerMovement.onWall())
        {
            // Reset fall height if the player touches a wall
            startY = transform.position.y;
        }
        else
        {
            if (!isFalling)
            {
                startY = transform.position.y;
                isFalling = true;
            }
        }
    }
}