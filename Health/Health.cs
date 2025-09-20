using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator animator;
    private bool dead;

    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float flashNumber;
    private SpriteRenderer spriteRend;

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;

    [Header ("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    private bool invulnerable;
    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                // Deactivates all attached components
                foreach (Behaviour component in components)
                    component.enabled = false;
                animator.SetBool("Grounded", true);
                animator.SetTrigger("Death");
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        animator.ResetTrigger("Death");
        animator.Play("Idle");
        StartCoroutine(Invunerability());
        // Activates all attached components
        foreach (Behaviour component in components)
            component.enabled = true;
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(3, 8, true);
        for (int i = 0; i < flashNumber; i++)
        {
            yield return new WaitForSeconds(iFramesDuration / (flashNumber * 2));
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (flashNumber * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (flashNumber * 2));
        }
        Physics2D.IgnoreLayerCollision(3, 8, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void FallDeath()
    {
        currentHealth = 0;
        foreach (Behaviour component in components)
            component.enabled = false;
        animator.SetBool("Grounded", true);
        animator.SetTrigger("Death");
        dead = true;
        SoundManager.instance.PlaySound(deathSound);
    }
}
