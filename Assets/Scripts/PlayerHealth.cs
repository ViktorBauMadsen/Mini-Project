using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    // Total HP
    public int maxHealth = 5;         // total health in "HP"
    // Current HP
    public int currentHealth;         // HP changes
    // How many HP per heart icon
    public int healthPerHeart = 2;    // 2 = full heart

    [Header("UI")]
    // UI shown on defeat
    public GameObject defeatUI;
    // Full-screen damage flash image
    public Image damageFlashImage;

    [Header("Hearts UI")]
    // Heart prefab to instantiate
    public GameObject heartPrefab;        // assign your Heart prefab
    // Container for heart icons
    public Transform heartsContainer;     // where hearts will appear
    // Heart sprites
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Damage Flash Settings")]
    // Speed the flash fades out
    public float flashFadeSpeed = 5f;
    // Max alpha for the flash
    public float flashMaxAlpha = 0.35f;

    // Internal alpha value for flash
    private float flashAlpha = 0f;
    // Is the player dead?
    private bool isDead = false;

    private PlayerMovement movement;
    // Cached heart images
    private List<Image> heartImages = new List<Image>();


    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Hide defeat UI at start
        if (defeatUI != null)
            defeatUI.SetActive(false);

        // Reset flash alpha
        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0;
            damageFlashImage.color = c;
        }

        movement = GetComponent<PlayerMovement>();

        CreateHearts();
        UpdateHeartsUI();
    }
    // Heal the player
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHeartsUI(); // or whatever updates your UI            
    }

    // Create heart icons based on max health
    void CreateHearts()
    {
        int heartCount = Mathf.CeilToInt(maxHealth / (float)healthPerHeart);

        for (int i = 0; i < heartCount; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsContainer);
            Image img = h.GetComponent<Image>();
            heartImages.Add(img);
        }
    }

    // Update heart sprites to reflect current HP
    void UpdateHeartsUI()
    {
        int hp = currentHealth;

        for (int i = 0; i < heartImages.Count; i++)
        {
            if (hp >= healthPerHeart)
            {
                heartImages[i].sprite = fullHeart;
            }
            else if (hp == 1)
            {
                heartImages[i].sprite = halfHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }

            hp -= healthPerHeart;
        }
    }


    void Update()
    {
        // Fade damage flash over time
        if (flashAlpha > 0)
        {
            flashAlpha -= Time.unscaledDeltaTime * flashFadeSpeed;
            flashAlpha = Mathf.Clamp01(flashAlpha);

            Color c = damageFlashImage.color;
            c.a = flashAlpha * flashMaxAlpha;
            damageFlashImage.color = c;
        }

        // Press R to restart after death
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // Apply damage to player
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        // Trigger flash
        flashAlpha = 1f;

        UpdateHeartsUI(); // ❤️ NEW

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle player death
    void Die()
    {
        isDead = true;

        Time.timeScale = 0f;

        if (defeatUI != null)
            defeatUI.SetActive(true);

        if (movement != null)
            movement.FreezePlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Restart the current scene
    void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
