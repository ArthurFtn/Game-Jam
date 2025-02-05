using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;
    public float speed;
    protected Transform target;

    public int moneyReward = 50;
    public delegate void EnemyDeathHandler();
    public event EnemyDeathHandler OnDeath;


    private bool isSlowed = false;
    private float originalSpeed;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        originalSpeed = speed; // Store original speed
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log("Slowing enemy: " + gameObject.name);

        if (!isSlowed)
        {
            StartCoroutine(SlowEffect(slowFactor, duration));
        }
    }

    
    private IEnumerator SlowEffect(float slowFactor, float duration)
    {
        if (isSlowed) yield break; // Prevent multiple slow effects from stacking

        isSlowed = true;
        speed *= slowFactor;
        Debug.Log(gameObject.name + " slowed! New speed: " + speed);

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        isSlowed = false;
        Debug.Log(gameObject.name + " slow effect ended. Speed restored: " + speed);
    }

  protected void Die()
{
    Debug.Log(gameObject.name + " has died.");
    
    if (MoneyManager.instance != null)
    {
        MoneyManager.instance.AddMoney(moneyReward);
    }

    // Notify WaveManager
    OnDeath?.Invoke();

    Destroy(gameObject);
}
}
