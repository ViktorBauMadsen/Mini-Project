using UnityEngine;
using TMPro;

public class DamageUpgrade : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int baseCost = 50;
    public float costMultiplier = 1.5f;
    public int damageIncrease = 1;

    private int currentCost;

    [Header("References")]
    public TMP_Text costText;             // Price ($)
    public TMP_Text damageText;           // NEW: displays player damage
    public PlayerShooting playerShooting;

    private void Start()
    {
        currentCost = baseCost;
        UpdateUI();
    }

    public void BuyUpgrade()
    {
        if (ScoreManager.Instance.money < currentCost)
            return;

        // Pay for upgrade
        ScoreManager.Instance.AddMoney(-currentCost);

        // Increase damage
        playerShooting.normalDamage += damageIncrease;

        // Apply new projectile damage
        Projectile.defaultDamage = playerShooting.normalDamage;
        Projectile.damage = Projectile.defaultDamage;

        // Increase next cost
        currentCost = Mathf.RoundToInt(currentCost * costMultiplier);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (costText != null)
            costText.text = currentCost + "$";

        if (damageText != null)
            damageText.text = "Damage: " + playerShooting.normalDamage;
    }
}
