using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Singleton for global access
    public static UpgradeManager Instance;

    [Header("Persistent Damage Upgrade Data")]
    // Current player damage
    public int currentDamage = 1;
    // Cost for the next damage upgrade
    public int currentDamageUpgradeCost = 50;
    // Multiplier to increase upgrade cost after purchase
    public float damageUpgradeCostMultiplier = 1.5f;

    [Header("Persistent Money")]
    // Player money (persists across scenes)
    public int currentMoney = 0;

    private void Awake()
    {
        // Make this object persistent and unique
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Increase damage and raise the next upgrade cost
    public void ApplyDamageUpgrade(int amount)
    {
        currentDamage += amount;
        currentDamageUpgradeCost = Mathf.RoundToInt(
            currentDamageUpgradeCost * damageUpgradeCostMultiplier
        );
    }
}
