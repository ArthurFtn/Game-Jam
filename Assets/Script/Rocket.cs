using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float explosionRadius = 3f;
    public float lifetime = 5f;

    private Transform target;

    public void SetTarget(Transform enemyTarget)
    {
        target = enemyTarget;
    }

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy rocket if it doesn't hit anything
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Explosion Effect (Area Damage)
            Explode();

            // Destroy the rocket
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            if (nearby.CompareTag("Enemy"))
            {
                Enemy enemy = nearby.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage / 2); // Splash damage (half of direct hit)
                }
            }
        }
    }
}
