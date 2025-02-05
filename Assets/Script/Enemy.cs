using UnityEngine;

public abstract class Enemy : MonoBehaviour // Classe mère abstraite
{
    public float maxHealth;
    protected float currentHealth;
    public float speed;
    protected Transform target;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        
        
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}