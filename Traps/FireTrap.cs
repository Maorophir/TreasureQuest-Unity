using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header ("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private SpriteRenderer spriteRend;
    [Header ("SFX")]
    [SerializeField] private AudioClip FireSound;
    private bool triggered; // When the trap gets triggered
    private bool active; // When the trap is active

    private Health playerHealth;
    
    private CapsuleCollider2D capsule;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        capsule.enabled = false;
    }

    private void Update()
    {
        if (playerHealth != null && active) 
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
           if (!triggered)
           {
                StartCoroutine(ActivateFiretrap());
           }
           if(active)
            collision.GetComponent<Health>().TakeDamage(damage);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = null;
        }
    }
    private IEnumerator ActivateFiretrap()
    {   //Make the Trap red to notify triggered
        triggered = true;
        animator.SetTrigger("Trigger");
        //Wait for delay, activate trap, turn on animation, return color to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(FireSound);
        capsule.enabled = true;
        active = true;
        animator.SetBool("Activated", true);

        //Wait deactivate trap and reset all variables
        yield return new WaitForSeconds(activeTime);
        active = false;
        capsule.enabled = false;
        triggered = false;
        animator.SetBool("Activated", false);
    }
    
}
