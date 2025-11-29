using UnityEngine;

public class BoostMusicController : MonoBehaviour
{
    public AudioSource boostMusic;
    private AudioSource normalMusic;

    private void Awake()
    {
        // Find the music manager by tag
        GameObject mm = GameObject.FindWithTag("MusicManager");
        if (mm != null)
            normalMusic = mm.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // Mute the main game music
        if (normalMusic != null)
            normalMusic.mute = true;

        // Play boost music
        if (boostMusic != null)
            boostMusic.Play();
    }

    private void OnDisable()
    {
        // Stop boost music
        if (boostMusic != null)
            boostMusic.Stop();

        // Unmute main music
        if (normalMusic != null)
            normalMusic.mute = false;
    }
}
