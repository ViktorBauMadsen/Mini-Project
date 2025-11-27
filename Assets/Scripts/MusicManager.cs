using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);   // keep music across scenes
        }
        else
        {
            Destroy(gameObject);             // prevent duplicates
        }
    }
}
