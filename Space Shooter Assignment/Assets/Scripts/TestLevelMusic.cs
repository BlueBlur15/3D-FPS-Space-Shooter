using UnityEngine;
using UnityEngine.Rendering;

public class TestLevelMusic : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.testLevelMusic);
        }
    }
}
