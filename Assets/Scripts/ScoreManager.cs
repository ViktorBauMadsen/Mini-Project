using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int enemiesKilled = 0;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text moneyText;

    [Header("Money Settings")]
    public int moneyPerKill = 10;

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
        AddMoney(moneyPerKill);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        AddKill();
    }

    public void AddMoney(int amount)
    {
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
