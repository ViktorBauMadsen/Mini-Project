using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this from your Play button
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        SceneManager.LoadScene(nextIndex);
    }

    // Optional: load a specific scene by index
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    // New: load the first scene (build index 0)
    public void LoadFirstScene()
    {
        if (SceneManager.sceneCountInBuildSettings > 0)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.LogWarning("SceneLoader: No scenes found in Build Settings. Cannot load scene 0.");
        }
    }

    // Optional: quit game
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // lets Quit work in Editor
#endif
    }
}
