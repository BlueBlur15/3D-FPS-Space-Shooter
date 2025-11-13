using System.Collections;
using System.Collections.Generic;
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

        void OnDisable()
        {
            PlayerHealth ph = FindObjectOfType<PlayerHealth>();
            if (ph != null)
            {
                ph.OnPlayerDied -= HandleGameOver;
            }
        }

        void HandleGameOver()
        {
            Debug.Log("Game Over! (GameManagger heard the event)");
            SceneManager.LoadScene("LoseScene");
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
