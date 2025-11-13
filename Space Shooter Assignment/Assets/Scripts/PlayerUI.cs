using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;

    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnHealthOrLivesChanged += HandleHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthOrLivesChanged -= HandleHealthChanged;
        }
    }

    void HandleHealthChanged(int currentHealth, int lives)
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth;

        if (livesText != null)
                livesText.text = "Lives: " + lives;
    }
}
