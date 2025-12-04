using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Controls enemy wave spawning and boss spawns
public class WaveManager : MonoBehaviour
{
    [Header("Normal Enemy Settings")]
    // Regular enemy prefab
    public GameObject enemyPrefab;
    // Possible spawn locations
    public Transform[] spawnPoints;
    // Max time between waves
    public float timeBetweenWaves = 20f;

    [Header("Boss Settings")]
    // Boss prefab
    public GameObject bossPrefab;
    // Spawn a boss every X waves
    public int bossInterval = 5;
    // Base boss HP
    public int bossBaseHealth = 10;
    // Extra boss HP per wave
    public int bossHealthPerWave = 2;

    [Header("UI")]
    // UI text that shows current wave
    public TextMeshProUGUI waveText;

    // Current wave number
    private int currentWave = 0;
    // Timer tracking time since last wave
    private float waveTimer = 0f;
    // Track alive enemies so we know when a wave is cleared
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        // Begin first wave immediately
        StartNewWave();
    }

    void Update()
    {
        // Remove null entries (dead enemies) from the list
        aliveEnemies.RemoveAll(e => e == null);

        waveTimer += Time.deltaTime;

        // Start next wave if all enemies are dead or timer expired
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

        // Scale number of enemies and a small speed bonus
        int enemiesToSpawn = 2 + currentWave; // normal wave scaling
        float speedBonus = currentWave * 0.1f;

        // -------------------------
        // Spawn normal enemies
        // -------------------------
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Pick a random spawn point and create an enemy
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomSpawn.position, Quaternion.identity);

            aliveEnemies.Add(enemy);

            // Give enemies a small speed boost as waves progress
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && ai.agent != null)
                ai.agent.speed += speedBonus;

            // New balanced HP scaling per wave
            EnemyHealth hp = enemy.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                // Base HP = 3, +0.5 per wave (rounded)
                hp.maxHealth = Mathf.RoundToInt(3 + (currentWave - 1) * 0.5f);

                hp.currentHealth = hp.maxHealth;
            }
        }

        // -------------------------
        // Spawn BOSS every X waves
        // -------------------------
        if (currentWave % bossInterval == 0)
        {
            SpawnBoss();
        }

        Debug.Log($"Starting wave {currentWave} with {enemiesToSpawn} zombies.");
    }       

    void SpawnBoss()
    {
        // Create boss at a random spawn point
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject boss = Instantiate(bossPrefab, randomSpawn.position, Quaternion.identity);

        aliveEnemies.Add(boss);

        // Scale boss speed slightly with wave
        EnemyAI ai = boss.GetComponent<EnemyAI>();
        if (ai != null && ai.agent != null)
            ai.agent.speed += currentWave * 0.1f;

        // Apply boss HP scaling
        EnemyHealth hp = boss.GetComponent<EnemyHealth>();
        if (hp != null)
        {
            hp.isBoss = true;

            // BOSS SCALING (your original logic — works great)
            hp.maxHealth = bossBaseHealth + (currentWave * bossHealthPerWave);
            hp.currentHealth = hp.maxHealth;
        }

        Debug.Log($"BOSS SPAWNED with {hp.maxHealth} HP!");
    }
}
