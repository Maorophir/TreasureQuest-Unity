using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCast : MonoBehaviour
{
    [SerializeField] private float castCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;
    private Animator animator;
    private PlayerMovement_1 playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement_1>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && cooldownTimer > castCooldown && playerMovement.canCast())
        
            Cast();
        
        cooldownTimer += Time.deltaTime;
    }

    private void Cast()
    {
        SoundManager.instance.PlaySound(fireballSound);
        animator.SetTrigger("Cast");
        cooldownTimer = 0;
        StartCoroutine(Shoot());
        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(0.2f);
            fireballs[FindFireball()].transform.position = firePoint.position;
            fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }
    private int FindFireball()
    {
        for (int i = 0; i <fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

}
