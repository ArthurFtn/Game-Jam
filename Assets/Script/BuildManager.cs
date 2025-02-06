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

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();

        // Ajoute les coûts des tours
        towerCosts[MiniGun] = 100;
        towerCosts[Cannon] = 150;
        towerCosts[Tesla] = 150;
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

            if (Input.GetMouseButtonDown(0))
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
                }
                else
                {
                    Debug.Log("💰 Pas assez d'argent !");
                }

                Destroy(previewTower);
                isPlacing = false;
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Clic droit pour vendre
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
            if (towerToSell.CompareTag("Tower"))
            {
                int refundAmount = Mathf.RoundToInt(towerCosts[towerToSell] * 0.7f); // Rembourse 70%
                MoneyManager.instance.AddMoney(refundAmount);
                Destroy(towerToSell);
                Debug.Log("🔄 Tour vendue !");
            }
        }
    }
}
