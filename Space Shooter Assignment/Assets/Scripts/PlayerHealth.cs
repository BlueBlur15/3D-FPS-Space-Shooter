using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Lives")]
    public int lives = 3;
    public float respawnDelay = 1.5f;
    public Transform respawnPoint;          // optional, can be empty

    private bool isDead = false;

    // Events
    public event Action<int, int> OnHealthOrLivesChanged;   // (currentHealth, lives)
    public event Action OnPlayerDied;                       // fires when lives hit 0

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

        void Die()
        {
            isDead = true;
            lives--;

            Debug.Log("PLayer died. Lives left: " + lives);

            if (lives > 0)
            {
                // Simple respawn
                Invoke(nameof(respawnDelay), respawnDelay);
            }
            else
            {
                OnPlayerDied?.Invoke();
                Debug.Log("GAME OVER (placerholder)");
            }
        }
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            // If no respawn point, just stay where you died for now
            Debug.Log("No respawn point set. Player stays in place.");
        }
    }

    void RaiseHealthEvent()
    {
        OnHealthOrLivesChanged?.Invoke(currentHealth, lives);
    }
}
