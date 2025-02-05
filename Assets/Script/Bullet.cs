using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Bullet speed
    public float lifetime = 2f; // Destroy bullet after X seconds
    public float damage = 3f; // Damage per hit

    void Start()
    {
        Destroy(gameObject, lifetime); // Auto-destroy after lifetime expires
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move forward
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if bullet hits an enemy
        {
            Enemy enemy = other.GetComponent<Enemy>(); // Get enemy script
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Apply damage
            }

            Destroy(gameObject); // Destroy the bullet on impact
        }
    }
}
