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
                Instantiate(towerPrefab, gridPosition, Quaternion.identity);

                Debug.Log("✅ Tour placée à : " + gridPosition);
                // 🔥 On NE désactive PLUS `isPlacing`, pour placer plusieurs tours
            }
            else
            {
                Debug.Log("🚫 Rien touché !");
            }
        }
    }
}