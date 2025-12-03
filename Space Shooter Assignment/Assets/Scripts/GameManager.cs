using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            ph.OnPlayerDied += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            ph.OnPlayerDied -= HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over! (GameManager heard the event)");
        SceneManager.LoadScene("LoseScene");
    }
}
