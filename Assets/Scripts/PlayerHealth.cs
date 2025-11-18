using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public GameObject defeatUI; // Assign your Defeat Canvas here

    private bool isDead = false;
    private PlayerMovement movement; // Reference to disable movement

    void Start()
    {
        currentHealth = maxHealth;

        // Hide defeat UI on start
        if (defeatUI != null)
            defeatUI.SetActive(false);

        // Get movement script
        movement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Freeze world time
        Time.timeScale = 0f;

        // Show defeat UI
        if (defeatUI != null)
            defeatUI.SetActive(true);

        // Disable player movement & camera look
        if (movement != null)
            movement.FreezePlayer();

        // Keep cursor LOCKED + HIDDEN for consistency
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Player died. Press R to restart.");
    }

    void Update()
    {
        // Allow R restart after death
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        // Unfreeze time
        Time.timeScale = 1f;

        // Lock & hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
