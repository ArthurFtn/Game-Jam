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
    private GridManager gridManager; 

    private Dictionary<GameObject, int> towerCosts = new Dictionary<GameObject, int>(); // Store tower costs

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void SelectMiniGun() 
    {
        selectedTower = MiniGun;
        isPlacing = true;
        CreatePreviewTower();
    }

    public void SelectCannon() 
    {
        selectedTower = Cannon;
        isPlacing = true;
        CreatePreviewTower();
    }

    public void SelectTesla() 
    {
        selectedTower = Tesla;
        isPlacing = true;
        CreatePreviewTower();
    }

    public void SelectInferno() 
    {
        selectedTower = Inferno;
        isPlacing = true;
        CreatePreviewTower();
    }

    public void StopPlacingTower()
    {
        isPlacing = false;
        Destroy(previewTower);
    }

    void CreatePreviewTower()
    {
        if (selectedTower == null) return;

        if (previewTower != null)
        {
            Destroy(previewTower);
        }

        previewTower = Instantiate(selectedTower, Vector3.zero, Quaternion.identity);
        previewTower.GetComponent<Renderer>().material.color = Color.green; 
        previewTower.GetComponent<Collider>().enabled = false; 
    }

    void Update()
    {
        if (isPlacing && selectedTower != null)
        {
            if (previewTower != null)
            {
                Vector3 gridPosition = GetMousePositionOnGrid();
                previewTower.transform.position = gridPosition;

                if (Input.GetMouseButtonDown(0)) 
                {
                    if (IsGridOccupied(gridPosition)) 
                    {
                        Debug.Log("🚫 Emplacement occupé !");
                        return;
                    }

                    int towerCost = GetTowerCost(selectedTower);
                    if (MoneyManager.instance.CanAfford(towerCost))
                    {
                        MoneyManager.instance.SpendMoney(towerCost);
                        GameObject newTower = Instantiate(selectedTower, gridPosition, Quaternion.identity);
                        towerCosts[newTower] = towerCost; // Store tower cost for refund
                        newTower.tag = "Tower"; // Ensure towers are tagged correctly
                    }
                    else
                    {
                        Debug.Log("Pas assez d'argent !");
                    }

                    Destroy(previewTower);
                    isPlacing = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Right-click to sell a tower
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
            if (towerToSell.CompareTag("Tower") && towerCosts.ContainsKey(towerToSell))
            {
                int refundAmount = Mathf.RoundToInt(towerCosts[towerToSell] * 0.7f); // 70% refund
                MoneyManager.instance.AddMoney(refundAmount);
                towerCosts.Remove(towerToSell);
                Destroy(towerToSell);
                Debug.Log("Tour vendue ! Remboursement : " + refundAmount);
            }
        }
    }

    private int GetTowerCost(GameObject tower)
    {
        if (tower == MiniGun) return 100; 
        if (tower == Cannon) return 150;
        if (tower == Tesla) return 150;
        if (tower == Inferno) return 100;

        return 0;
    }
}
