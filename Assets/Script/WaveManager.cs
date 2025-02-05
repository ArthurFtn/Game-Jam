using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject bigEnemyPrefab;
    public GameObject midEnemyPrefab;
    public GameObject smallEnemyPrefab;
    public Transform spawnPoint; // Point de spawn des ennemis

    public int initialBigEnemies = 2;
    public int initialMidEnemies = 3;
    public int initialSmallEnemies = 5;

    private int currentWave = 0;
    private int enemiesRemaining = 0;

    public float spawnInterval = 1.5f; // Temps entre chaque spawn

    public delegate void WaveChangeHandler(int waveNumber);
    public event WaveChangeHandler OnWaveChanged;

    private void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (enemiesRemaining > 0) return; // On attend que tous les ennemis soient morts

        currentWave++;
        int bigEnemies = initialBigEnemies + currentWave;
        int midEnemies = initialMidEnemies + currentWave * 2;
        int smallEnemies = initialSmallEnemies + currentWave * 3;

        StartCoroutine(SpawnWave(bigEnemies, midEnemies, smallEnemies));

        OnWaveChanged?.Invoke(currentWave); // Mise à jour UI (si nécessaire)
    }

    private IEnumerator SpawnWave(int big, int mid, int small)
    {
        enemiesRemaining = big + mid + small;

        for (int i = 0; i < big; i++)
        {
            SpawnEnemy(bigEnemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int i = 0; i < mid; i++)
        {
            SpawnEnemy(midEnemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int i = 0; i < small; i++)
        {
            SpawnEnemy(smallEnemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            Debug.Log("Tous les ennemis sont morts, vague terminée !");
        }
    }
}
