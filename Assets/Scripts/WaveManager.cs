using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 20f;

    [Header("UI")]
    public TextMeshProUGUI waveText;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        StartNewWave();
    }

    void Update()
    {
        // Check if all enemies are dead
        aliveEnemies.RemoveAll(e => e == null);

        waveTimer += Time.deltaTime;

        if (aliveEnemies.Count == 0 || waveTimer >= timeBetweenWaves)
        {
            StartNewWave();
        }
    }

    void StartNewWave()
    {
        currentWave++;
        waveTimer = 0f;

        waveText.text = $"Wave: {currentWave}";

        int enemiesToSpawn = 2 + currentWave; // waves get harder

        // Movement bonus per wave
        float speedBonus = currentWave * 0.1f;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomSpawn.position, Quaternion.identity);

            aliveEnemies.Add(enemy);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && ai.agent != null)
            {
                ai.agent.speed += speedBonus;
            }

            // Every 5 waves → +1 health
            EnemyHealth hp = enemy.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                if (currentWave % 5 == 0)
                    hp.maxHealth++;

                hp.currentHealth = hp.maxHealth;
            }
        }

        Debug.Log($"Starting wave {currentWave} with {enemiesToSpawn} zombies");
    }
}
