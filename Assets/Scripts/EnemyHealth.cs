using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;

    public bool isBoss = false;

    public GameObject healthBarPrefab;
    private EnemyHealthBarUI healthBarUI;

    void Start()
    {
        currentHealth = maxHealth;

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
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBarUI != null)
            healthBarUI.UpdateFill();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // Add score
        ScoreManager.Instance.AddScore(1);

        if (healthBarUI != null)
            Destroy(healthBarUI.gameObject);

        Destroy(gameObject);
    }
}
