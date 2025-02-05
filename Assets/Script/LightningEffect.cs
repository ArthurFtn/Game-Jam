using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float duration = 0.2f;

    public void Initialize(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        Destroy(gameObject, duration); // Remove effect after duration
    }
}
