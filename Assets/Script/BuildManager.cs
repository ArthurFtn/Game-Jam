using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject MiniGun; // Tour 1
    public GameObject Cannon;  // Tour 2
    private GameObject selectedTower; // 🔥 Tour sélectionnée

    private bool isPlacing = false; // Mode placement
    public LayerMask groundLayer; // Layer pour le sol
    private GridManager gridManager; // Référence au gestionnaire de grille

    public int miniGunCost = 50; // Coût de la MiniGun
    public int cannonCost = 75;  // Coût de la Cannon

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void SelectMiniGun() // 🔥 Appelé par le bouton MiniGun
    {
        if (MoneyManager.instance.CanAfford(miniGunCost)) // Vérifie si le joueur peut se permettre la tour
        {
            selectedTower = MiniGun;
            isPlacing = true;
            MoneyManager.instance.SpendMoney(miniGunCost); // Déduit l'argent lors de la sélection
            Debug.Log("MiniGun sélectionné !");
        }
        else
        {
            Debug.Log("Pas assez d'argent pour MiniGun !");
        }
    }

    public void SelectCannon() // 🔥 Appelé par le bouton Cannon
    {
        if (MoneyManager.instance.CanAfford(cannonCost)) // Vérifie si le joueur peut se permettre la tour
        {
            selectedTower = Cannon;
            isPlacing = true;
            MoneyManager.instance.SpendMoney(cannonCost); // Déduit l'argent lors de la sélection
            Debug.Log("Cannon sélectionné !");
        }
        else
        {
            Debug.Log("Pas assez d'argent pour Cannon !");
        }
    }

    public void StopPlacingTower()
    {
        isPlacing = false;
        Debug.Log("Mode placement désactivé !");
    }

    void Update()
    {
        if (!isPlacing || selectedTower == null) return;

        if (Input.GetMouseButtonDown(0)) // Clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Debug.Log("✅ Raycast touché : " + hit.collider.name);

                Vector3 gridPosition = gridManager.GetNearestGridPosition(hit.point);

                if (IsGridOccupied(gridPosition))
                {
                    Debug.Log("🚫 Cet emplacement est déjà occupé !");
                    return;
                }

                Instantiate(selectedTower, gridPosition, Quaternion.identity); // 🔥 Instancie la tour sélectionnée
                Debug.Log($"✅ {selectedTower.name} placée à : " + gridPosition);
                
                isPlacing = false; // Désactiver le mode placement après avoir posé une tour
            }
            else
            {
                Debug.Log("🚫 Rien touché !");
            }
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
}
