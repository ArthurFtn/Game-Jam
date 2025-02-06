using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private bool isOccupied = false;
    private Renderer rend;
    public Material MatDisponible;
    public Material MatIndisponible;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateColor();
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (rend != null)
        {
            rend.material = isOccupied ? MatIndisponible : MatDisponible;
        }
    }
}