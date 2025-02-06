using UnityEngine;

public class CannonAttack : MonoBehaviour, ITowerAttack
{
    public int cost = 150; // Ajout du coût de la tour
    public float attackRange = 7f;
    public float attackCooldown = 1f; // Plus lent que MiniGun
    private float lastAttackTime;
    public GameObject cannonballPrefab; // Projectile de la tour
    public Transform firePoint;
    public GameObject skinPrefab;
    private GameObject currentSkin;

    private bool canAttack = true; // Contrôle l'attaque
void Start()
    {
        if (skinPrefab != null)
        {
            currentSkin = Instantiate(skinPrefab, transform);
            currentSkin.transform.localPosition = Vector3.zero; // Align to tower position
        }
    }
    
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
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            cannonball.transform.LookAt(enemy.transform);
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