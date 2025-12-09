using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You have fallen. Respawning...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}