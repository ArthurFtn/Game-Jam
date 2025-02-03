using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bigEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject smallEnemyPrefab;
    
    public float spawnRate = 2f; // Temps entre chaque spawn
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        GameObject enemyPrefab = null;

        switch (randomEnemy)
        {
            case 0:
                enemyPrefab = bigEnemyPrefab;
                break;
            case 1:
                enemyPrefab = mediumEnemyPrefab;
                break;
            case 2:
                enemyPrefab = smallEnemyPrefab;
                break;
        }

        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}