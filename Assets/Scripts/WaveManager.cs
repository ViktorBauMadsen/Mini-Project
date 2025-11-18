using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 3f;
    public int enemiesPerWave = 3;

    private int waveNumber = 0;
    private bool spawningWave = false;

    private void Update()
    {
        // If no enemies exist, start next wave
        if (!spawningWave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        spawningWave = true;
        waveNumber++;

        int enemiesToSpawn = enemiesPerWave + (waveNumber - 1);

        Debug.Log("Spawning Wave " + waveNumber + " with " + enemiesToSpawn + " enemies");

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f); // small delay between each enemy
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        spawningWave = false;
    }

    void SpawnEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, point.position, Quaternion.identity);
    }
}
