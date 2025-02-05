using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour // Classe mère abstraite
{
    public float maxHealth;
    protected float currentHealth;
    public float speed; // Default movement speed
    protected Transform target;

    private bool isSlowed = false;
    private float originalSpeed; // Stores the original speed before slow effect

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        originalSpeed = speed; // Save the initial speed
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        if (!isSlowed) // Prevent multiple slow effects stacking
        {
            StartCoroutine(SlowEffect(slowFactor, duration));
        }
    }

    private IEnumerator SlowEffect(float slowFactor, float duration)
    {
        isSlowed = true;
        speed *= slowFactor; // Reduce speed
        yield return new WaitForSeconds(duration);
        speed = originalSpeed; // Restore original speed
        isSlowed = false;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
