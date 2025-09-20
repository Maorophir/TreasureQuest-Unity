using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Health playerHealth;
    private Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb = collision.GetComponent<Rigidbody2D>();
            playerHealth = collision.GetComponent<Health>();
            rb.velocity = Vector2.zero;
            playerHealth.FallDeath();
        }

    }
}
