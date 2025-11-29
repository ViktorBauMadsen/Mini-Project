using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;

    public bool isBoss = false;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private EnemyHealthBarUI healthBarUI;

    [Header("Damage Flash")]
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    private Renderer[] renderers;
    private List<Color[]> originalColors = new List<Color[]>();

    [Header("Particles")]
    public GameObject hitParticlePrefab;
    public GameObject deathParticlePrefab;

    void Start()
    {
        currentHealth = maxHealth;

        // --- UI setup (unchanged logic) ---
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab);
            healthBarUI = bar.GetComponent<EnemyHealthBarUI>();
            healthBarUI.enemy = this;
        }
        else
        {
            Debug.LogError("No health bar prefab assigned on enemy: " + name);
        }

        // --- Get all renderers (no material cloning!) ---
        renderers = GetComponentsInChildren<Renderer>();

        // --- Save original colors safely ---
        foreach (Renderer r in renderers)
        {
            Material[] mats = r.materials;
            Color[] stored = new Color[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].HasProperty("_BaseColor"))
                    stored[i] = mats[i].GetColor("_BaseColor");
                else if (mats[i].HasProperty("_Color"))
                    stored[i] = mats[i].GetColor("_Color");
            }

            originalColors.Add(stored);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // --- Hit particles ---
        if (hitParticlePrefab != null)
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

        // --- Red flash ---
        StartCoroutine(DamageFlash());

        // --- Update UI (unchanged) ---
        if (healthBarUI != null)
            healthBarUI.UpdateFill();

        // --- Death logic (unchanged) ---
        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator DamageFlash()
    {
        // Apply flash color
        for (int rIndex = 0; rIndex < renderers.Length; rIndex++)
        {
            Material[] mats = renderers[rIndex].materials; // SAFE: Unity creates instances automatically

            for (int m = 0; m < mats.Length; m++)
            {
                if (mats[m].HasProperty("_BaseColor"))
                    mats[m].SetColor("_BaseColor", flashColor);
                if (mats[m].HasProperty("_Color"))
                    mats[m].SetColor("_Color", flashColor);
            }
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original colors
        for (int rIndex = 0; rIndex < renderers.Length; rIndex++)
        {
            Material[] mats = renderers[rIndex].materials;
            Color[] cols = originalColors[rIndex];

            for (int m = 0; m < mats.Length; m++)
            {
                if (mats[m].HasProperty("_BaseColor"))
                    mats[m].SetColor("_BaseColor", cols[m]);
                if (mats[m].HasProperty("_Color"))
                    mats[m].SetColor("_Color", cols[m]);
            }
        }
    }

    private void Die()
    {
        // --- Death particles ---
        if (deathParticlePrefab != null)
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

        // --- Original Score logic ---
        ScoreManager.Instance.AddScore(1);

        // --- Remove health bar ---
        if (healthBarUI != null)
            Destroy(healthBarUI.gameObject);

        // --- Destroy the entire enemy (same as original script) ---
        Destroy(gameObject);
    }
}
