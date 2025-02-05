using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign enemy prefab in Inspector
    public Transform[] spawnPoints; // Array of spawn points
    public int enemiesPerWave = 10; // Number of enemies per wave
    public float timeBetweenWaves = 5f; // Time between waves

    private int currentWave = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
            enemiesSpawned = 0;
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f); // Delay between spawns
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemiesSpawned++;
        enemiesAlive++;

        enemy.GetComponent<Enemy>().OnDeath += EnemyDied;
    }

    void EnemyDied()
    {
        enemiesAlive--;
    }
}
