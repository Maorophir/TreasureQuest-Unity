using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeHeadDown : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination;
    private Vector3 originalPosition;
    private bool attacking;
    private bool reset;
    private Vector3[] directions = new Vector3[4];

    [Header ("SFX")]
    [SerializeField] private AudioClip impactSound;
    
    // private void OnEnable()
    // {
    //     Stop();
    // }
    
    private void Awake()
    {
        originalPosition = transform.position;
        reset = false;
    }

    private void Update()
    {   
        //Move spikehead to destination only if attacking
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);

        else if (reset == false)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        CalculateDirections();
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        // directions[0] = transform.right * range; //Right
        // directions[1] = -transform.right * range; //Left
        // directions[2] = transform.up * range; // Up
        directions[3] = -transform.up * range; // Down
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(1f); // Delay before returning

        // Move back to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = originalPosition; // Snap to original position
        reset = false;
    }

    new private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        Stop();//Stop after hitting
    }

        private void Stop()
    {
        destination = transform.position;
        reset = true;
        attacking = false;
        StartCoroutine(ReturnToOriginalPosition());
    }
}
