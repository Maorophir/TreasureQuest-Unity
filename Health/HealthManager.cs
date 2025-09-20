using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public float playerHealth;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize player health
            if (PlayerPrefs.HasKey("PlayerHealth"))
            {
                playerHealth = PlayerPrefs.GetFloat("PlayerHealth", 3f); // Default to 3 HP if no saved health is found
            }
            else
            {
                playerHealth = 3f; // Default starting health
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to update player health in the manager
    public void SetPlayerHealth(float newHealth)
    {
        playerHealth = Mathf.Clamp(newHealth, 0, 3f); // Clamp health to valid range (0 to max HP)
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth); // Save the updated health
    }

    // Method to retrieve player health from the manager
    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    // Optionally, you could add a method to reset health
    public void ResetHealth()
    {
        playerHealth = 3f; // Reset to full health
        PlayerPrefs.SetFloat("PlayerHealth", playerHealth); // Save the reset health
    }
}


