using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OxygenSystem : MonoBehaviour
{
    [SerializeField] private OxygenSystemStats oxygenSystemStats;

    private LineRenderer lineRenderer;
    private int segments = 50; // Defines the smoothness of the circle

    private void Awake()
    {
        // Set up LineRenderer component for the green border
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    private void Start()
    {
        UpdateRadius();
    }

    private void Update()
    {
        DepleteEnergy();
        UpdateRadius();
    }

    private void DepleteEnergy()
    {
        // Deplete energy at the configured rate from the ScriptableObject
    }

    private void UpdateRadius()
    {
        if (oxygenSystemStats != null)
        {
            float radius = oxygenSystemStats.Radius;
            DrawCircle(radius);
        }
    }

    private void DrawCircle(float radius)
    {
        // Calculate the points around the circle
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
