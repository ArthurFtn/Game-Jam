using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject MiniGun; // Tour 1
    public GameObject Cannon;  // Tour 2
    private GameObject selectedTower; // Tour sélectionnée
    private GameObject previewTower; // Prévisualisation de la tour
    private bool isPlacing = false; // Mode placement
    public LayerMask groundLayer; // Layer pour le sol
    private GridManager gridManager; // Référence au gestionnaire de grille

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void SelectMiniGun() // Appelé par le bouton MiniGun
    {
        selectedTower = MiniGun;
        if (selectedTower == null)
        {
            Debug.LogError("La tour MiniGun n'est pas assignée dans l'inspecteur !");
        }
        isPlacing = true;
        CreatePreviewTower(); // Créer la prévisualisation de la tour
        Debug.Log("MiniGun sélectionné !");
    }

    public void SelectCannon() // Appelé par le bouton Cannon
    {
        selectedTower = Cannon;
        if (selectedTower == null)
        {
            Debug.LogError("La tour Cannon n'est pas assignée dans l'inspecteur !");
        }
        isPlacing = true;
        CreatePreviewTower(); // Créer la prévisualisation de la tour
        Debug.Log("Cannon sélectionné !");
    }

    public void StopPlacingTower()
    {
        isPlacing = false;
        Destroy(previewTower); // Supprimer la prévisualisation si le placement est annulé
        Debug.Log("Mode placement désactivé !");
    }

    void CreatePreviewTower()
    {
        if (selectedTower == null)
        {
            Debug.LogError("Aucune tour sélectionnée !"); // Ajoute un message d'erreur si la tour n'est pas sélectionnée.
            return; // Ne pas essayer de prévisualiser si aucune tour n'est sélectionnée.
        }

        if (previewTower != null)
        {
            Destroy(previewTower); // Supprimer l'ancienne prévisualisation s'il y en a une
        }

        // Instancier la tour sélectionnée comme prévisualisation
        previewTower = Instantiate(selectedTower, Vector3.zero, Quaternion.identity);
        previewTower.GetComponent<Renderer>().material.color = Color.green; // Changer la couleur de la prévisualisation pour la rendre visible
        previewTower.GetComponent<Collider>().enabled = false; // Désactiver le collider pour qu'elle ne bloque pas le placement

        // Désactiver l'attaque sur la prévisualisation
        DisableTowerAttack(previewTower);
    }

    void DisableTowerAttack(GameObject tower)
    {
        if (tower == null) return;

        ITowerAttack attackScript = null;

        // Parcourir tous les MonoBehaviour pour trouver ITowerAttack
        foreach (MonoBehaviour script in tower.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script is ITowerAttack)
            {
                attackScript = (ITowerAttack)script;
                break;
            }
        }

        if (attackScript != null)
        {
            attackScript.DisableAttack();
            Debug.Log("🔴 Attaque désactivée pour la prévisualisation.");
        }
        else
        {
            Debug.LogWarning("⚠️ Aucun script ITowerAttack trouvé sur la tour.");
        }
    }


    void Update()
    {
        if (!isPlacing || selectedTower == null) return;

        if (previewTower != null)
        {
            Vector3 gridPosition = GetMousePositionOnGrid(); // Obtenir la position du curseur sur la grille
            previewTower.transform.position = gridPosition; // Déplacer la prévisualisation

            if (Input.GetMouseButtonDown(0)) // Clique gauche
            {
                if (IsGridOccupied(gridPosition)) // Vérifier si l'emplacement est occupé
                {
                    Debug.Log("🚫 Cet emplacement est déjà occupé !");
                    return;
                }

                Instantiate(selectedTower, gridPosition, Quaternion.identity); // Poser la tour
                Destroy(previewTower); // Supprimer la prévisualisation
                isPlacing = false; // Désactiver le mode placement après avoir posé la tour
            }
        }
    }

    private bool IsGridOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f); // Vérifie si l'emplacement est occupé
        foreach (Collider collider in colliders)
        {
            // Ignorer la prévisualisation de la tour (éviter que la prévisualisation soit détectée comme une tour placée)
            if (collider.gameObject == previewTower) continue;

            if (collider.CompareTag("Tower"))
            {
                return true; // Si un collider avec le tag "Tower" est trouvé, l'emplacement est occupé
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
            return gridManager.GetNearestGridPosition(hit.point); // Retourner la position sur la grille
        }

        return Vector3.zero; // Retourner la position par défaut si rien n'est touché
    }
}
