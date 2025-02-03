using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TeslaTower : MonoBehaviour
{
    public float damage = 50f;
    public float attackRange = 10f;
    public float attackCooldown = 1f;
    public LayerMask enemyLayer;

    private float lastAttackTime;

    void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformChainAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformChainAttack()
    {
        // Find enemies in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hitColliders.Length == 0) return;

        // Sort enemies by distance
        var sortedEnemies = hitColliders
            .Select(c => c.GetComponent<Enemy>())
            .Where(e => e != null)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .ToList();

        // Attack first 3 enemies
        for (int i = 0; i < Mathf.Min(3, sortedEnemies.Count); i++)
        {
            sortedEnemies[i].TakeDamage(damage);
        }
    }
}

// Assuming you have a basic Enemy script
public class Enemy : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}