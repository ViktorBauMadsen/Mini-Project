using UnityEngine;

// Ensures time and audio are reset when a scene starts
public class ResetTimeScaleOnLoad : MonoBehaviour
{
    void Start()
    {
        // Resume normal game time
        Time.timeScale = 1f;
        // Unpause audio globally
        AudioListener.pause = false;
        // Restore default master volume
        AudioListener.volume = 1f;
    }
}
