using UnityEngine;
using System;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 5;
    [SerializeField] public int currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(1f);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        // Normalize health ratio for UI
        float healthRatio = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(healthRatio);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
    }
}