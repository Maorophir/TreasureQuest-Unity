using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        // Check if there is a checkpoint available
        if (currentCheckpoint == null)
        {
            //Show game over screen
            uiManager.GameOver();
            return;
        }
        // Sets a small offset to the y position to ensure the player spawns above ground
        Vector3 respawnPosition = currentCheckpoint.position;
        respawnPosition.y += 1f; // Adjusts the value

        transform.position = respawnPosition; // Move player to current checkpoint
        playerHealth.Respawn(); // Restore player health and reset animation

        // Move camera to checkpoint
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    // Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint" && playerHealth.currentHealth != 0)
        {
            currentCheckpoint = collision.transform; // Store checkpoint location
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; // Deactivate checkpoint Collider
        }

        if (collision.transform.tag == "FCheckpoint") // First invisible Checkpoint
        {
            currentCheckpoint = collision.transform; // Store checkpoint location
            collision.GetComponent<Collider2D>().enabled = false; // Deactivate checkpoint Collider
        }
    }
}
