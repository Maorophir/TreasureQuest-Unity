using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGlow : MonoBehaviour
{
    private SpriteRenderer glow;
    void Awake()
    {
        glow = GetComponent<SpriteRenderer>();
        glow.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            glow.enabled = true;
        }
    }
}
