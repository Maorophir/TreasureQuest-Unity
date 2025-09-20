using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [Header ("Main Menu")]
    [SerializeField] private MainMenu mainMenuScreen;
    private GameObject thisScreen;
    private void Awake()
    {
        // thisScreen = GameObject.con
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            mainMenuScreen.Return(gameObject);
    }
}
