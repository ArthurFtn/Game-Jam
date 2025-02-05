using UnityEngine;

public class TowerMinigun : MonoBehaviour
{
    public float damage = 3f; // Lower damage per shot
    public float attackRange = 5f;
    public float attackCooldown = 0.1f; // Faster attack speed
    private float lastAttackTime;

    public GameObject bulletPrefab; // Assign in Inspector
    public Transform firePoint; // Empty GameObject for bullet spawn

    void Update()
    {
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
            // Spawn a bullet at firePoint
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Make the bullet face the enemy
            bullet.transform.LookAt(enemy.transform);

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
}
