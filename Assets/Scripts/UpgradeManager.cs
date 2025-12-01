using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Persistent Damage Upgrade Data")]
    public int currentDamage = 1;
    public int currentDamageUpgradeCost = 50;
    public float damageUpgradeCostMultiplier = 1.5f;

    [Header("Persistent Money")]
    public int currentMoney = 0;

    private void Awake()
    {
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

    public void ApplyDamageUpgrade(int amount)
    {
        currentDamage += amount;
        currentDamageUpgradeCost = Mathf.RoundToInt(
            currentDamageUpgradeCost * damageUpgradeCostMultiplier
        );
    }
}
