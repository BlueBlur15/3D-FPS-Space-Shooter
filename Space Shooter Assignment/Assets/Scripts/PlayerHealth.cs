using System;
using UnityEngine;
using UnityEngine.SceneManagement;   // <-- IMPORTANT

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Lives")]
    public int lives = 3;
    public float respawnDelay = 1.5f;
    public Transform respawnPoint;   // assign in Inspector

    private bool isDead = false;

    // Optional events (you can ignore these if not using them)
    public event Action<int, int> OnHealthOrLivesChanged;
    public event Action OnPlayerDied;

    private void Start()
    {
        currentHealth = maxHealth;
        RaiseHealthEvent();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Player took damage. HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        RaiseHealthEvent();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        lives--;

        Debug.Log("Player died. Lives left: " + lives);

        if (lives > 0)
        {
            Debug.Log("Respawning in " + respawnDelay + " seconds...");
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            Debug.Log("PLAYER OUT OF LIVES — loading LoseScene");
            OnPlayerDied?.Invoke();   // optional, for GameManager if you still use it

            // Change to LoseScene later once we make the UI for it
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;

        // Try to get a CharacterController on this object or a parent
        CharacterController controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = GetComponentInParent<CharacterController>();
        }

        if (respawnPoint != null)
        {
            Debug.Log("Respawning player at respawnPoint: " + respawnPoint.position);

            if (controller != null)
            {
                // Temporarily disable controller so we can safely teleport
                controller.enabled = false;
                transform.position = respawnPoint.position;
                controller.enabled = true;
            }
            else
            {
                // No CharacterController found, just move the transform
                transform.position = respawnPoint.position;
            }
        }
        else
        {
            Debug.LogWarning("No respawn point set. Player stays where they died.");
        }

        RaiseHealthEvent();
    }

    private void RaiseHealthEvent()
    {
        OnHealthOrLivesChanged?.Invoke(currentHealth, lives);
    }
}
