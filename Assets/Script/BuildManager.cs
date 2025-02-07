using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public GameObject Inferno;
    public GameObject Tesla;
    public GameObject MiniGun;
    public GameObject Cannon;
    private GameObject selectedTower;
    private GameObject previewTower;
    private bool isPlacing = false;
    public LayerMask groundLayer;
    public GridManager gridManager;

    private Dictionary<GameObject, int> towerCosts = new Dictionary<GameObject, int>(); // Stocke le coût des tours
    private Dictionary<GameObject, int> placedTowers = new Dictionary<GameObject, int>(); // Stocke les tours placées et leur coût

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();

        // Ajoute les coûts des tours
        towerCosts[MiniGun] = 100;
        towerCosts[Cannon] = 100;
        towerCosts[Tesla] = 100;
        towerCosts[Inferno] = 100;
    }

    public void SelectMiniGun() { StartPlacing(MiniGun); }
    public void SelectCannon() { StartPlacing(Cannon); }
    public void SelectTesla() { StartPlacing(Tesla); }
    public void SelectInferno() { StartPlacing(Inferno); }

    void StartPlacing(GameObject tower)
    {
        selectedTower = tower;
        isPlacing = true;
        CreatePreviewTower();
    }

    void CreatePreviewTower()
    {
        if (selectedTower == null) return;

        if (previewTower != null)
            Destroy(previewTower);

        previewTower = Instantiate(selectedTower);
        previewTower.GetComponent<Collider>().enabled = false; // Désactive le collider
        previewTower.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f); // Rend la tour transparente
    }

    void Update()
    {
        if (isPlacing && selectedTower != null)
        {
            Vector3 gridPosition = GetMousePositionOnGrid();
            if (previewTower != null)
            {
                previewTower.transform.position = gridPosition;
            }

            if (Input.GetMouseButtonDown(0)) // Clic gauche pour placer la tour
            {
                if (IsGridOccupied(gridPosition)) 
                {
                    Debug.Log("🚫 Emplacement occupé !");
                    return;
                }

                int towerCost = towerCosts[selectedTower];
                if (MoneyManager.instance.CanAfford(towerCost))
                {
                    MoneyManager.instance.SpendMoney(towerCost);
                    GameObject newTower = Instantiate(selectedTower, gridPosition, Quaternion.identity);
                    newTower.tag = "Tower"; 

                    // ✅ Enregistre la tour placée et son coût
                    placedTowers.Add(newTower, towerCost);
                }
                else
                {
                    Debug.Log("💰 Pas assez d'argent !");
                }

                Destroy(previewTower);
                isPlacing = false;
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Clic droit pour vendre une tour
        {
            SellTower();
        }
    }

    private bool IsGridOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetMousePositionOnGrid()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Caméra principale non trouvée !");
            return Vector3.zero;
        }

        if (gridManager == null)
        {
            Debug.LogError("GridManager non initialisé !");
            return Vector3.zero;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            return gridManager.GetNearestGridPosition(hit.point);
        }

        return Vector3.zero;
    }

    public void SellTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject towerToSell = hit.collider.gameObject;
            Debug.Log("🎯 Objet touché par le raycast : " + towerToSell.name);

            if (towerToSell.CompareTag("Tower"))
            {
                Debug.Log("💰 Tour détectée : " + towerToSell.name);

                // ✅ Chercher le coût de la tour placée
                if (placedTowers.ContainsKey(towerToSell))
                {
                    int refundAmount = Mathf.RoundToInt(placedTowers[towerToSell] * 0.7f);
                    MoneyManager.instance.AddMoney(refundAmount);

                    // ✅ Supprimer la tour du dictionnaire avant de la détruire
                    placedTowers.Remove(towerToSell);
                    Destroy(towerToSell);

                    Debug.Log("✅ Tour vendue, remboursement de " + refundAmount);
                }
                else
                {
                    Debug.LogError("❌ Impossible de trouver le coût de cette tour !");
                }
            }
            else
            {
                Debug.Log("❌ L'objet touché n'est pas une tour !");
            }
        }
        else
        {
            Debug.Log("❌ Aucune tour détectée !");
        }
    }
}
