using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] private AudioClip rockslideSound;
    [SerializeField] private Image blackScreen; // Reference to the black screen Image
    [SerializeField] private float fadeDuration = 1f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rockslideSound;

        // Ensure the black screen is initially inactive
        blackScreen.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Start the transition sequence
            StartCoroutine(TransitionSequence());
        }
    }

    private IEnumerator TransitionSequence()
    {
        // Enable the black screen
        blackScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeBlackScreen(true, fadeDuration));

        // Play the transition sound
        audioSource.Play();

        // Wait until the sound finishes playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Load the next level
        SceneManager.LoadScene("Level 2");

        // Start fading out after the scene has loaded
        yield return StartCoroutine(FadeBlackScreen(false, fadeDuration));

        // Deactivate the black screen after fading out
        blackScreen.gameObject.SetActive(false);
    }


    private IEnumerator FadeBlackScreen(bool fadeIn, float duration)
    {
        float fadeAmount;
        Color screenColor = blackScreen.color;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeAmount = fadeIn ? (t / duration) : (1 - t / duration);
            screenColor.a = fadeAmount;
            blackScreen.color = screenColor;
            yield return null;
        }

        // Ensure the final alpha is set correctly
        screenColor.a = fadeIn ? 1 : 0;
        blackScreen.color = screenColor;
    }
}
