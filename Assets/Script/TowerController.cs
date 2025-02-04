using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    public int chainAttacks = 3;
    public float chainDelay = 0.5f;

    public GameObject projectilePrefab;
    public LayerMask enemyLayer;

    private bool canAttack = true;

    void Update()
    {
        // Find closest enemy within range
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            attackRange,
            enemyLayer
        );

        if (hitColliders.Length > 0 && canAttack)
        {
            StartCoroutine(PerformChainAttack(hitColliders[0].transform));
        }
    }

    IEnumerator PerformChainAttack(Transform target)
    {
        canAttack = false;

        // Perform chain attacks
        for (int i = 0; i < chainAttacks; i++)
        {
            // Spawn and launch projectile
            GameObject projectile = Instantiate(
                projectilePrefab,
                transform.position,
                Quaternion.identity
            );

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(target);
            }

            // Wait between chain attacks
            yield return new WaitForSeconds(chainDelay);
        }

        // Reset attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Visualize attack range in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}