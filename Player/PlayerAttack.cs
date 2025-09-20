using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Enemy Layer")]
    [SerializeField] private LayerMask enemyLayer;

    [Header ("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    private float cooldownTimer = Mathf.Infinity;
    private Animator animator;
    private Health enemyHealth;
    private PlayerMovement_1 playerMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement_1>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.F) && cooldownTimer > attackCooldown && playerMovement.isGrounded() && !PlayerMovement_1.isCrouching)
        {   
                Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        SoundManager.instance.PlaySound(attackSound);
        cooldownTimer = 0;

    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range
        * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, enemyLayer);
        if (hit.collider != null)
            enemyHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamageEnemy()
    {
        if (EnemyInSight() && enemyHealth != null)
            enemyHealth.TakeDamage(damage);
    }
}
