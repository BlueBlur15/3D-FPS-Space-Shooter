using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Lives")]
    public int lives = 3;
    public Transform respawnPoint;   // assign in Inspector

    private bool isDead = false;

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
            // 🔥 INSTANT RESPAWN — removed delay & Invoke
            Respawn();
        }
        else
        {
            Debug.Log("PLAYER OUT OF LIVES — loading LoseScene");
            OnPlayerDied?.Invoke();

            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;

        CharacterController controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = GetComponentInParent<CharacterController>();

        if (respawnPoint != null)
        {
            Debug.Log("Respawning player at: " + respawnPoint.position);

            if (controller != null)
            {
                controller.enabled = false;
                transform.position = respawnPoint.position;
                controller.enabled = true;
            }
            else
            {
                transform.position = respawnPoint.position;
            }
        }
        else
        {
            Debug.LogWarning("No respawn point set!");
        }

        RaiseHealthEvent();
    }

    private void RaiseHealthEvent()
    {
        OnHealthOrLivesChanged?.Invoke(currentHealth, lives);
    }
}
