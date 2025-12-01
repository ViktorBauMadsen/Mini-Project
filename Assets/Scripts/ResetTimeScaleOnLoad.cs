using UnityEngine;

public class ResetTimeScaleOnLoad : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }
}
