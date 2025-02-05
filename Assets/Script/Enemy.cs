using UnityEngine;

public abstract class Enemy : MonoBehaviour // Classe mère abstraite
{
    public float maxHealth;
    protected float currentHealth;
    public float speed;
    protected Transform target;

    public int moneyReward = 50; // Récompense en argent quand l'ennemi est tué

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
        MoneyManager.instance.AddMoney(moneyReward); // Ajoute l'argent au joueur lors de la mort de l'ennemi
        Destroy(gameObject);
    }
}