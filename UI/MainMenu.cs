using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header ("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;

    [Header ("Screens")]
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject creditsScreen;

    private void Awake()
    {
        mainMenuScreen.SetActive(true);
    }
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Controls()
    {
        controlsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }
    public void Return(GameObject _screen)
    {
        mainMenuScreen.SetActive(true);
        _screen.SetActive(false);
    }

        public void Credits()
    {
        creditsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();// Quits the game (only works on build)
        #if UNITY_EDITOR       
            UnityEditor.EditorApplication.isPlaying = false; //Exites play mode (while in editor)
        #endif
    }
}
