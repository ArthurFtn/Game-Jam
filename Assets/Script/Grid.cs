using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10, 10); // Taille de la grille
    public float cellSize = 1f; // Taille d'une case
    public LayerMask groundLayer; // Layer du sol

    public Vector3 GetNearestGridPosition(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, 0, z);
    }
}