using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject towerPrefab; // Tour à placer
    private bool isPlacing = false; // Mode placement
    public LayerMask groundLayer; // Layer pour le sol
    private GridManager gridManager; // Référence au gestionnaire de grille

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void StartPlacingTower()
    {
        isPlacing = true;
        Debug.Log("Mode placement activé !");
    }
    
    public void StopPlacingTower()
    {
        isPlacing = false;
        Debug.Log("Mode placement désactivé !");
    }

    void Update()
    {
        if (!isPlacing) return;

        if (Input.GetMouseButtonDown(0)) // Clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) // 🔥 Détecte UNIQUEMENT le sol
            {
                Debug.Log("✅ Raycast touché : " + hit.collider.name);

                Vector3 gridPosition = gridManager.GetNearestGridPosition(hit.point); // 🔥 Convertit la position en case

                // Vérifier si une tour est déjà présente à cet endroit
                if (IsGridOccupied(gridPosition))
                {
                    Debug.Log("🚫 Cet emplacement est déjà occupé !");
                    return; // Ne pas instancier la tour si l'emplacement est occupé
                }

                Instantiate(towerPrefab, gridPosition, Quaternion.identity);

                Debug.Log("✅ Tour placée à : " + gridPosition);
            }
            else
            {
                Debug.Log("🚫 Rien touché !");
            }
        }
    }

    // Fonction pour vérifier si l'emplacement est occupé
    private bool IsGridOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f); // Vérifie si des objets se trouvent à proximité
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Tower")) // Assure-toi que tes tours ont le tag "Tower"
            {
                return true; // Un objet avec le tag "Tower" a été trouvé à cette position
            }
        }
        return false; // Aucune tour n'est présente à cet endroit
    }
}
