using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton instance
    private static MusicManager instance;

    void Awake()
    {
        // If no instance exists, set this and keep it across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);   // keep music across scenes
        }
        else
        {
            // If another instance exists, destroy this duplicate
            Destroy(gameObject);             // prevent duplicates
        }
    }
}
