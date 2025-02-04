using UnityEngine;

public class MiniGunAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 0.1f; // Faster firing speed
    private float lastAttackTime;

    public GameObject bulletPrefab; // Assign in Unity Inspector
    public Transform firePoint; // Position where bullets spawn

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
