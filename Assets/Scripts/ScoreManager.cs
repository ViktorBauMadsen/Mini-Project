using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int enemiesKilled = 0;
    public TMP_Text scoreText;

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
        UpdateUI();
    }

    // ⭐ This fixes your issue — EnemyHealth calls AddScore(1)
    public void AddScore(int amount)
    {
        AddKill();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Enemies Killed: " + enemiesKilled;
    }
}
