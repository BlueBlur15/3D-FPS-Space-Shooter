using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScene : MonoBehaviour
{
    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickInstructionsButton()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
        Debug.Log("Quit Button Works");
    }
}
