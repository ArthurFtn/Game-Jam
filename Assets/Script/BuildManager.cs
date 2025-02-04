using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject MiniGun; // Tour 1
    public GameObject Cannon;  // Tour 2
    private GameObject selectedTower; // 🔥 Tour sélectionnée

    private bool isPlacing = false; // Mode placement
    public LayerMask groundLayer; // Layer pour le sol
    private GridManager gridManager; // Référence au gestionnaire de grille

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void SelectMiniGun() // 🔥 Appelé par le bouton MiniGun
    {
        selectedTower = MiniGun;
        isPlacing = true;
        Debug.Log("MiniGun sélectionné !");
    }

    public void SelectCannon() // 🔥 Appelé par le bouton Cannon
    {
        selectedTower = Cannon;
        isPlacing = true;
        Debug.Log("Cannon sélectionné !");
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
