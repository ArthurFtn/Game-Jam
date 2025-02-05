using UnityEngine;

public class TowerRocket : MonoBehaviour, ITowerAttack
{
    public float attackRange = 7f;
    public float attackCooldown = 2f; // Slower fire rate
    private float lastAttackTime;

    public GameObject rocketPrefab; // Assign in Unity Inspector
    public Transform firePoint; // Position where rockets spawn
    public AudioSource shootAudio;

    private bool canAttack = true; // Contrôle l'attaque

    void Update()
    {
        if (!canAttack) return; // Désactive l'attaque si nécessaire

        Enemy enemy = FindClosestEnemy();
        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Attack(enemy);
        }
    }

    void Attack(Enemy enemy)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Spawn a rocket at firePoint
            GameObject rocketObj = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
            
            // Assign the enemy as the rocket's target
            Rocket rocket = rocketObj.GetComponent<Rocket>();
            if (rocket != null)
            {
                rocket.SetTarget(enemy.transform);
            }

            if (shootAudio != null)
            {
                shootAudio.Play();
            }
            lastAttackTime = Time.time;
        }
    }

    Enemy FindClosestEnemy()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (enemies.Length == 0) return null;

        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    // Implémentation de ITowerAttack
    public void DisableAttack()
    {
        canAttack = false;
    }

    public void EnableAttack()
    {
        canAttack = true;
    }
}
