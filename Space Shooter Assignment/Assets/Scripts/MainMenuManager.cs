using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Level one");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void InstructionsButton()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
