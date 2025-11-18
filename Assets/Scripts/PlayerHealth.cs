using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;         // total health in "HP"
    public int currentHealth;         // HP changes
    public int healthPerHeart = 2;    // 2 = full heart

    [Header("UI")]
    public GameObject defeatUI;
    public Image damageFlashImage;

    [Header("Hearts UI")]
    public GameObject heartPrefab;        // assign your Heart prefab
    public Transform heartsContainer;     // where hearts will appear
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Damage Flash Settings")]
    public float flashFadeSpeed = 5f;
    public float flashMaxAlpha = 0.35f;

    private float flashAlpha = 0f;
    private bool isDead = false;

    private PlayerMovement movement;
    private List<Image> heartImages = new List<Image>();


    void Start()
    {
        currentHealth = maxHealth;

        if (defeatUI != null)
            defeatUI.SetActive(false);

        // reset flash alpha
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
        if (flashAlpha > 0)
        {
            flashAlpha -= Time.unscaledDeltaTime * flashFadeSpeed;
            flashAlpha = Mathf.Clamp01(flashAlpha);

            Color c = damageFlashImage.color;
            c.a = flashAlpha * flashMaxAlpha;
            damageFlashImage.color = c;
        }

        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        flashAlpha = 1f;

        UpdateHeartsUI(); // ❤️ NEW

        if (currentHealth <= 0)
        {
            Die();
        }
    }

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

    void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
