using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header ("Boss Health")]
    [SerializeField] private Health bigBossHealth;
    [SerializeField] private float minionsAppear;

    [Header ("Minions")]
    [SerializeField] private GameObject[] minions;

    [Header ("Minions Apeear Sound")]
    [SerializeField] private AudioClip BirthSound;
    private bool activated = false;
    
    private void Update()
    {
        if (bigBossHealth.currentHealth == minionsAppear && !activated)
        {
            WakeUp();
        }
    }

    private void WakeUp()
    {
        activated = true;
        SoundManager.instance.PlaySound(BirthSound);
        foreach (GameObject minion in minions)
            minion.SetActive(true);
    }
}
