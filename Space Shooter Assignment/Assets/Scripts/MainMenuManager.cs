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

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelOneButton()
    {
        SceneManager.LoadScene("Level one");
    }

    public void LevelTwoButton()
    {
        SceneManager.LoadScene("Level Two");
    }
}
