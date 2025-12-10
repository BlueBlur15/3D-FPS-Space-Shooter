using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.mainMenuMusic);
        }
    }
}