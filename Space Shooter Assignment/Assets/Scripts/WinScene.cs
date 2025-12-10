using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    public void OnClickLevelOneButton()
    {
        SceneManager.LoadScene("Level one");
    }

    public void OnClickLevelTwoButton()
    {
        SceneManager.LoadScene("Level Two");
    }

    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
        Debug.Log("Quit Button Works");
    }
}
