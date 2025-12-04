using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    // Pause menu canvas
    [Header("Assign your pause menu canvas here")]
    public GameObject pauseMenu;

    // Is the game currently paused?
    private bool isPaused = false;

    // Watch for Escape to toggle pause
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Show pause UI, stop time and audio, show cursor
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Pause all audio
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Hide pause UI, resume time and audio, hide cursor
    public void ResumeGame()
    {   
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Resume all audio
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        // Unpause everything BEFORE leaving the scene
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene("MainMenu");
    }
}
