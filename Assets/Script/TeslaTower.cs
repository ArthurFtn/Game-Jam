using UnityEngine;
using System.Collections.Generic;

public class TeslaTower : MonoBehaviour
{
    public float attackRange = 7f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    public GameObject lightningEffectPrefab; 
    public int maxChainTargets = 3;
    public int damagePerTarget = 20;

    public float slowFactor = 0.5f; // 50% speed reduction
    public float slowDuration = 2f; // Slows enemies for 2 seconds

    void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            ChainLightningAttack();
        }
    }

    void ChainLightningAttack()
    {
        Enemy[] enemies = FindEnemiesInRange();
        if (enemies.Length == 0) return;

        lastAttackTime = Time.time;

        Enemy previousTarget = null;
        Vector3 startPoint = transform.position;

        for (int i = 0; i < Mathf.Min(maxChainTargets, enemies.Length); i++)
        {
            Enemy target = enemies[i];

            // Apply damage
            target.TakeDamage(damagePerTarget);

            // Apply slow effect
            target.ApplySlow(slowFactor, slowDuration);

            // Spawn Lightning Effect
            SpawnLightningEffect(startPoint, target.transform.position);

            // Move starting point for next chain jump
            startPoint = target.transform.position;
        }
    }

    void SpawnLightningEffect(Vector3 start, Vector3 end)
    {
        if (lightningEffectPrefab != null)
        {
            GameObject lightning = Instantiate(lightningEffectPrefab, start, Quaternion.identity);
            LightningEffect effect = lightning.GetComponent<LightningEffect>();
            if (effect != null)
            {
                effect.Initialize(start, end);
            }
        }
    }

    Enemy[] FindEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Enemy enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange.ToArray();
    }
}
