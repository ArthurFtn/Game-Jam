using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 20f;
    public float speed = 10f;
    private Transform target;

    public void SetTarget(Transform enemyTransform)
    {
        target = enemyTransform;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Check if reached target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Get enemy component and deal damage
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy projectile
        Destroy(gameObject);
    }
}
