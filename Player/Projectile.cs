using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField] private int damage;
    private float direction;
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private float lifetime;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit)return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0); 
        lifetime += Time.deltaTime;
        if (lifetime > 1)
        {
            hit = true;
            animator.SetTrigger("Explode");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Transparent")
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("Explode");
        }
            if (collision.tag == "Enemy")
                collision.GetComponent<Health>()?.TakeDamage(damage);
        
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.lossyScale.y, transform.localScale.z);
    }

    private void Deactive()
    {
        gameObject.SetActive(false);
    }
    
}
