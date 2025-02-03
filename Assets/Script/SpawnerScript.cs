using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bigEnemyPrefab;
    public GameObject midEnemyPrefab;
    public GameObject smallEnemyPrefab;

    public float bigEnemySpawnRate = 3f;  // Temps entre chaque spawn de gros ennemis
    public float midEnemySpawnRate = 2f;  // Temps entre chaque spawn d'ennemis moyens
    public float smallEnemySpawnRate = 1f; // Temps entre chaque spawn de petits ennemis

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Spawn un gros ennemi à intervalle régulier
            yield return new WaitForSeconds(bigEnemySpawnRate);
            Instantiate(bigEnemyPrefab, transform.position, Quaternion.identity);

            // Spawn un ennemi moyen à intervalle régulier
            yield return new WaitForSeconds(midEnemySpawnRate);
            Instantiate(midEnemyPrefab, transform.position, Quaternion.identity);

            // Spawn un petit ennemi à intervalle régulier
            yield return new WaitForSeconds(smallEnemySpawnRate);
            Instantiate(smallEnemyPrefab, transform.position, Quaternion.identity);
        }
    }
}