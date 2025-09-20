using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockslide : MonoBehaviour
{
    [SerializeField] private AudioClip avalancheSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            SoundManager.instance.PlaySound(avalancheSound);
    }
    
}
