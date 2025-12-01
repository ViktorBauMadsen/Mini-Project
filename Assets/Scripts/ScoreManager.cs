using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int enemiesKilled = 0;

    [Header("UI References")]
    public TMP_Text scoreText;      // "Enemies Killed"
    public TMP_Text moneyText;      // "$123"

    [Header("Money Settings")]
    public int money = 0;           // current currency
    public int moneyPerKill = 10;   // how much each kill is worth

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddKill()
    {
        enemiesKilled++;

        // ⭐ Add money when killing enemy
        AddMoney(moneyPerKill);

        UpdateUI();
    }

    // This method is already used by your EnemyHealth script
    public void AddScore(int amount)
    {
        AddKill();
    }

    // ⭐ Add money directly (in case you want loot drops later)
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Enemies Killed: " + enemiesKilled;

        if (moneyText != null)
            moneyText.text = "$" + money;
    }
}
