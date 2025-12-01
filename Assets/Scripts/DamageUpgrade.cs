using UnityEngine;
using TMPro;

public class DamageUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int damageIncrease = 1;

    [Header("References")]
    public TMP_Text costText;
    public TMP_Text damageText;
    public PlayerShooting playerShooting;

    private void Start()
    {
        UpdateUI();
    }

    public void BuyUpgrade()
    {
        int cost = UpgradeManager.Instance.currentDamageUpgradeCost;

        // Not enough money
        if (UpgradeManager.Instance.currentMoney < cost)
            return;

        // Pay
        UpgradeManager.Instance.currentMoney -= cost;

        // Increase damage & cost
        UpgradeManager.Instance.ApplyDamageUpgrade(damageIncrease);

        // Apply new permanent damage
        playerShooting.normalDamage = UpgradeManager.Instance.currentDamage;
        Projectile.defaultDamage = UpgradeManager.Instance.currentDamage;
        Projectile.damage = UpgradeManager.Instance.currentDamage;

        // Refresh UI
        UpdateUI();
        ScoreManager.Instance.UpdateUI();
    }

    private void UpdateUI()
    {
        costText.text = UpgradeManager.Instance.currentDamageUpgradeCost + "$";
        damageText.text = "Damage: " + UpgradeManager.Instance.currentDamage;
    }
}
