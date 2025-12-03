using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Lives")]
    public int lives = 3;
    public float respawnDelay = 1.5f;
    public Transform respawnPoint;

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
        RaiseHealthEvent();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        lives--;

        Debug.Log("Player died. Lives left: " + lives);
        RaiseHealthEvent();

        if (lives > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            OnPlayerDied?.Invoke();
            Debug.Log("GAME OVER (placeholder)");
        }
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }

        RaiseHealthEvent();
    }

    private void RaiseHealthEvent()
    {
        OnHealthOrLivesChanged?.Invoke(currentHealth, lives);
    }
}
