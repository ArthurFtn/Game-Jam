using UnityEngine;

public class SellableTower : MonoBehaviour
{
    public int buildCost = 100; // Prix de construction de la tour
    private int sellPrice;

    void Start()
    {
        sellPrice = Mathf.FloorToInt(buildCost * 0.5f); // Prix de vente = 50% du coût de construction
    }

    public void Sell()
    {
        // Ajouter l'argent au joueur
        MoneyManager.instance.AddMoney(sellPrice);
        Destroy(gameObject); // Détruire la tour
        Debug.Log($"Tour vendue pour {sellPrice} !");
    }
}