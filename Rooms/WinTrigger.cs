using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Header ("Wining Screen")]
    [SerializeField] private GameObject WiningScreen;
    [SerializeField] private AudioClip WinGameSound;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.SetActive(false);
        SoundManager.instance.PlaySound(WinGameSound);
        WiningScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
