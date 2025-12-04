using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Max health
    public int maxHealth = 3;
    // Current health (hidden in inspector)
    [HideInInspector] public int currentHealth;

    // Is this a boss?
    public bool isBoss = false;

    // Prefab for world-space health bar
    public GameObject healthBarPrefab;
    private EnemyHealthBarUI healthBarUI;

    [Header("Damage Flash")]
    // How long the damage flash lasts
    public float flashDuration = 0.1f;
    // Color used when flashing on hit
    public Color flashColor = Color.red;

    // Renderers to flash
    private Renderer[] renderers;
    // Original colors for restoring after flash
    private List<Color[]> originalColors = new List<Color[]>();

    [Header("Particles")]
    // Particle shown on hit
    public GameObject hitParticlePrefab;
    // Particle shown on death
    public GameObject deathParticlePrefab;

    // Boost powerup drop
    [Header("Powerup Drop")]
    public GameObject powerupPrefab;
    [Range(0f, 1f)] public float dropChance = 0.05f;

    // Health pickup drop
    [Header("Health Drop")]
    public GameObject healthPickupPrefab;
    [Range(0f, 1f)] public float healthDropChance = 0.05f;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Spawn health bar UI if provided
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab);
            healthBarUI = bar.GetComponent<EnemyHealthBarUI>();
            healthBarUI.enemy = this;
        }

        // Cache renderers and store original colors
        renderers = GetComponentsInChildren<Renderer>();

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

    // Apply damage to this enemy
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (hitParticlePrefab != null)
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

        StartCoroutine(DamageFlash());

        if (healthBarUI != null)
            healthBarUI.UpdateFill();

        if (currentHealth <= 0)
            Die();
    }

    // Flash enemy materials briefly when hit
    private IEnumerator DamageFlash()
    {
        for (int rIndex = 0; rIndex < renderers.Length; rIndex++)
        {
            Material[] mats = renderers[rIndex].materials;
            for (int m = 0; m < mats.Length; m++)
            {
                if (mats[m].HasProperty("_BaseColor"))
                    mats[m].SetColor("_BaseColor", flashColor);
                if (mats[m].HasProperty("_Color"))
                    mats[m].SetColor("_Color", flashColor);
            }
        }

        yield return new WaitForSeconds(flashDuration);

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

    // Handle death: particles, drops, scoring, cleanup
    private void Die()
    {
        if (deathParticlePrefab != null)
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

        TryDropPowerup();
        TryDropHealthPickup();

        ScoreManager.Instance.AddScore(1);

        if (healthBarUI != null)
            Destroy(healthBarUI.gameObject);

        Destroy(gameObject);
    }
      
    // Maybe drop a powerup based on chance
    private void TryDropPowerup()
    {
        if (powerupPrefab == null) return;

        float roll = Random.value;
        if (roll <= dropChance)
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
    }

    // Maybe drop a health pickup based on chance
    private void TryDropHealthPickup()
    {
        if (healthPickupPrefab == null) return;

        float roll = Random.value;
        if (roll <= healthDropChance)
        {
            Instantiate(
                healthPickupPrefab,
                new Vector3(transform.position.x, 1f, transform.position.z),
                Quaternion.identity
            );
        }
    }
}
