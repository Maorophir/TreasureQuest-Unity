using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform arrowPoint;
    [SerializeField] private GameObject[] arrows;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;
    [SerializeField] private float triggerDistance = 10f; // Distance to trigger sound
    [SerializeField] private float maxVolumeDistance = 2f; // Distance at which sound is at full volume

    private float cooldownTimer;
    private Transform player;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming your player has the tag "Player"
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = arrowSound;
        audioSource.playOnAwake = false;
    }

    private void Shoot()
    {
        cooldownTimer = 0;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= triggerDistance)
        {
            float volume = Mathf.Lerp(1f, 0f, (distanceToPlayer - maxVolumeDistance) / (triggerDistance - maxVolumeDistance));
            volume = Mathf.Clamp(volume, 0f, 1f);
            audioSource.volume = volume;
            audioSource.Play();
        }

        arrows[FindArrow()].transform.position = arrowPoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Shoot();
        }
    }
}
