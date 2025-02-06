using UnityEngine;
using UnityEngine.UI;

public class BaseHealthUI : MonoBehaviour
{
    public BaseHealth baseHealth; // Référence au script de la base
    public Image healthBarFill; // Référence à l'Image de la barre de vie (la verte)

    private void Start()
    {
        if (baseHealth != null)
        {
            baseHealth.OnHealthChanged += UpdateHealthBar; // Écoute l'événement
            UpdateHealthBar((float)baseHealth.currentHealth / baseHealth.maxHealth); // Met à jour au début
        }
    }

    private void UpdateHealthBar(float fillAmount)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = fillAmount;
        }
    }
}