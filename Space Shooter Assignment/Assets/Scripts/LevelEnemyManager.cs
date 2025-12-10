using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelEnemyManager : MonoBehaviour
{
    public static LevelEnemyManager instance;

    [Header("Enemy/Level Settings")]
    public int totalEnemies = 0;                 // optional, auto-filled if 0
    public string nextSceneName;                 // e.g. "Level Two" or "WinScene"

    [Header("UI")]
    public TextMeshProUGUI enemiesLeftText;      // drag your "Enemies Left" text here

    private int enemiesRemaining;

    private void Awake()
    {
        // Simple singleton for this level
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // If not set manually, count all objects tagged "Enemy"
        if (totalEnemies <= 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            totalEnemies = enemies.Length;
        }

        enemiesRemaining = totalEnemies;
        UpdateEnemiesUI();
    }

    public void EnemyKilled()
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        UpdateEnemiesUI();

        if (enemiesRemaining == 0)
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private void UpdateEnemiesUI()
    {
        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = "Enemies Left: " + enemiesRemaining;
        }
    }

    private System.Collections.IEnumerator LoadNextLevel()
    {
        // Small delay so player can see "Enemies Left: 0"
        yield return new WaitForSeconds(2f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("LevelEnemyManager: nextSceneName is empty.");
        }
    }
}
