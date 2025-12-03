using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI livesText;

    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        // Find the PlayerHealth in the scene
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnHealthOrLivesChanged += HandleHealthChanged;

            // Initialize immediately so UI is filled even before damage
            HandleHealthChanged(playerHealth.currentHealth, playerHealth.lives);
        }
        else
        {
            Debug.LogWarning("PlayerUI: No PlayerHealth found in scene.");
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthOrLivesChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(int currentHealth, int lives)
    {
        Debug.Log($"PlayerUI: Updating UI. Health={currentHealth}, Lives={lives}");

        if (healthText != null)
            healthText.text = "Health: " + currentHealth;

        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
}
