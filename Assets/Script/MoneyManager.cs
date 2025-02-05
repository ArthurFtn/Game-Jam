using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance; // Instance unique pour le gestionnaire d'argent
    public int currentMoney = 100; // Montant initial de l'argent
    public TextMeshProUGUI moneyText; // Référence à un texte UI pour afficher l'argent

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + currentMoney.ToString(); // Affichage de l'argent
        }
    }

    // Ajoute de l'argent au joueur
    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    // Vérifie si le joueur a assez d'argent
    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }

    // Déduit de l'argent du joueur
    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            currentMoney -= amount;
        }
        else
        {
            Debug.Log("Pas assez d'argent !");
        }
    }
}