using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton access
    public static ScoreManager Instance;

    // Number of enemies killed
    private int enemiesKilled = 0;

    [Header("UI References")]
    // UI showing kills
    public TMP_Text scoreText;
    // UI showing money
    public TMP_Text moneyText;

    [Header("Money Settings")]
    // Money awarded per enemy kill
    public int moneyPerKill = 10;

    private void Awake()
    {
        // Set singleton instance
        Instance = this;
    }

    private void Start()
    {
        // Initialize UI
        UpdateUI();
    }

    public void AddKill()
    {
        // Increment kills, give money and refresh UI
        enemiesKilled++;
        AddMoney(moneyPerKill);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        // Alias: treat score addition as a kill
        AddKill();
    }

    public void AddMoney(int amount)
    {
        // Add to persistent money in UpgradeManager
        UpgradeManager.Instance.currentMoney += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Enemies Killed: " + enemiesKilled;

        if (moneyText != null)
            moneyText.text = "$" + UpgradeManager.Instance.currentMoney;
    }
}
