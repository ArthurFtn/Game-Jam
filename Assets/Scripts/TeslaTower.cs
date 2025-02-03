using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 1.5f;
    public int damage = 20;
    public int chainTargets = 2;

    private float attackTimer = 0f;
    public LayerMask enemyLayer;

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        if (hitColliders.Length == 0) return;

        // Get main target (closest enemy)
        Transform mainTarget = GetClosestEnemy(hitColliders);
        if (mainTarget == null) return;

        List<Transform> chainedTargets = GetChainedTargets(mainTarget, hitColliders);
        DealDamage(mainTarget);
        foreach (var target in chainedTargets)
        {
            DealDamage(target);
        }

        // Optional: Add visual effects for lightning chains
        StartCoroutine(ShowLightningEffect(mainTarget, chainedTargets));
    }

    Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    List<Transform> GetChainedTargets(Transform mainTarget, Collider[] enemies)
    {
        List<Transform> targets = new List<Transform>();
        foreach (Collider enemy in enemies)
        {
            if (enemy.transform == mainTarget) continue;

            if (targets.Count < chainTargets)
            {
                targets.Add(enemy.transform);
            }
        }
        return targets;
    }

    void DealDamage(Transform enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    System.Collections.IEnumerator ShowLightningEffect(Transform mainTarget, List<Transform> chainedTargets)
    {
        // Implement visual effect with LineRenderer or VFX here
        yield return new WaitForSeconds(0.2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
