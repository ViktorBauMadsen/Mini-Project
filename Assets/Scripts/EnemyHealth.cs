using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;

    public bool isBoss = false;

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

    // 🔥 Existing boost powerup drop
    [Header("Powerup Drop")]
    public GameObject powerupPrefab;                // assign in inspector
    [Range(0f, 1f)] public float dropChance = 0.05f; // default 5% chance

    // 🔥 NEW: Health item drop
    [Header("Health Drop")]
    public GameObject healthPickupPrefab;               // assign in inspector
    [Range(0f, 1f)] public float healthDropChance = 0.05f; // default 5%

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab);
            healthBarUI = bar.GetComponent<EnemyHealthBarUI>();
            healthBarUI.enemy = this;
        }

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

    private void Die()
    {
        // Death particle
        if (deathParticlePrefab != null)
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

        // 🔥 Drop boost powerup
        TryDropPowerup();

        // 🔥 Drop health pickup
        TryDropHealthPickup();

        // Score
        ScoreManager.Instance.AddScore(1);

        // Remove health bar
        if (healthBarUI != null)
            Destroy(healthBarUI.gameObject);

        // Destroy enemy
        Destroy(gameObject);
    }

    // 🔥 Boost powerup drop method
    private void TryDropPowerup()
    {
        if (powerupPrefab == null) return;

        float roll = Random.value; // number between 0 and 1
        if (roll <= dropChance)
        {
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
    }

    // 🔥 NEW: Health pickup drop method
    private void TryDropHealthPickup()
    {
        if (healthPickupPrefab == null) return;

        float roll = Random.value; // number between 0 and 1
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
