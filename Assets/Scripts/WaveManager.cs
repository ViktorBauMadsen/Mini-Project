using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Normal Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 20f;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public int bossInterval = 5;
    public int bossBaseHealth = 10;
    public int bossHealthPerWave = 2;

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
        // Remove references to dead enemies
        aliveEnemies.RemoveAll(e => e == null);

        waveTimer += Time.deltaTime;

        // Start next wave if all enemies died OR timer hit max
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

        int enemiesToSpawn = 2 + currentWave; // normal wave scaling

        float speedBonus = currentWave * 0.1f;

        // -------------------------
        // Spawn normal enemies
        // -------------------------
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomSpawn.position, Quaternion.identity);

            aliveEnemies.Add(enemy);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && ai.agent != null)
                ai.agent.speed += speedBonus;

            EnemyHealth hp = enemy.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                // Normal scaling
                if (currentWave % 5 == 0)
                    hp.maxHealth++;

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
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject boss = Instantiate(bossPrefab, randomSpawn.position, Quaternion.identity);

        aliveEnemies.Add(boss);

        EnemyAI ai = boss.GetComponent<EnemyAI>();
        if (ai != null && ai.agent != null)
            ai.agent.speed += currentWave * 0.1f;

        EnemyHealth hp = boss.GetComponent<EnemyHealth>();
        if (hp != null)
        {
            hp.isBoss = true;
            hp.maxHealth = bossBaseHealth + (currentWave * bossHealthPerWave);
            hp.currentHealth = hp.maxHealth;
        }

        Debug.Log($"BOSS SPAWNED with {hp.maxHealth} HP!");
    }
}
