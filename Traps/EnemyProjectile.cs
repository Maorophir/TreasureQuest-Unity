using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : EnemyDamage // Will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator animator;
    private bool hit;
    private CapsuleCollider2D coll;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
        // Reset velocity
        rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }
    
    new private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);//Execute logic from parent script first
        coll.enabled = false;
        
        if (animator != null)
            animator.SetTrigger("Explode"); // When the object is a fireball
        else
            gameObject.SetActive(false); // When this hits any object
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
