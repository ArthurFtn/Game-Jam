using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject bigEnemyPrefab;
    public GameObject midEnemyPrefab;
    public GameObject smallEnemyPrefab;
    public Transform spawnPoint; // Point de spawn des ennemis
    public Transform[] waypoints; // Tableau des waypoints
    public float timer = 0; // Timer animation 

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

        // Création d'une liste contenant tous les ennemis de la vague
        List<GameObject> enemiesToSpawn = new List<GameObject>();

        for (int i = 0; i < big; i++) enemiesToSpawn.Add(bigEnemyPrefab);
        for (int i = 0; i < mid; i++) enemiesToSpawn.Add(midEnemyPrefab);
        for (int i = 0; i < small; i++) enemiesToSpawn.Add(smallEnemyPrefab);

        // Mélange aléatoire des ennemis
        ShuffleList(enemiesToSpawn);

        // Spawn des ennemis dans l'ordre mélangé
        foreach (GameObject enemyPrefab in enemiesToSpawn)
        {
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Assigner les waypoints à l'ennemi
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            enemyMove.waypoints = waypoints;
        }
        if (timer <= 3)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Debug.LogError($"❌ L'ennemi {enemyPrefab.name} ne possède pas le script EnemyMove !");
        }

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

    // Fonction pour mélanger une liste
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap
        }
    }
}
