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
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            target = baseObject.transform;
        }
    }

    protected virtual void Update()
    {
        MoveTowardsTarget();
    }

    protected void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
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