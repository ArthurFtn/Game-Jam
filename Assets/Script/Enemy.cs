using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour // Classe mère abstraite
{
    public float maxHealth;
    protected float currentHealth;
    public float speed;
    protected Transform target;

    public int moneyReward = 50; // Récompense en argent quand l'ennemi est tué

    private bool isSlowed = false;
    private float originalSpeed; // Sauvegarde de la vitesse originale

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        originalSpeed = speed; // Stocke la vitesse de base
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
        if (!isSlowed) // Évite d'empiler plusieurs ralentissements
        {
            StartCoroutine(SlowEffect(slowFactor, duration));
        }
    }

    private IEnumerator SlowEffect(float slowFactor, float duration)
    {
        isSlowed = true;
        speed *= slowFactor; // Réduit la vitesse
        yield return new WaitForSeconds(duration);
        speed = originalSpeed; // Rétablit la vitesse initiale
        isSlowed = false;
    }

    protected void Die()
    {
        MoneyManager.instance.AddMoney(moneyReward); // Ajoute l'argent au joueur lors de la mort de l'ennemi
        Destroy(gameObject);
    }
}
