using UnityEngine;

public class TowerTesla : MonoBehaviour
{
    public float damage = 10f;
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    void Update()
    {
        Enemy enemy = FindClosestEnemy();
        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Attack(enemy);
        }
    }

    Enemy FindClosestEnemy()
    {
        // 🔥 La clé : récupérer TOUS les objets qui héritent de Enemy (y compris BigEnemy, MediumEnemy, SmallEnemy)
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

    void Attack(Enemy enemy)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            enemy.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}