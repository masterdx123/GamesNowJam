using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(CircleCollider2D))]
public class OxygenSystem : MonoBehaviour
{
    [SerializeField] private OxygenSystemStats oxygenSystemStats;

    private LineRenderer lineRenderer;
    private CircleCollider2D oxygenBubbleCollider;
    private int segments = 50; // Defines the smoothness of the circle
    private float currentEnergy;

    private void Awake()
    {
        // Set up LineRenderer component for the green border
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the material to avoid pink color
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        // Initialize and configure the CircleCollider2D for the oxygen bubble
        oxygenBubbleCollider = GetComponent<CircleCollider2D>();
        oxygenBubbleCollider.isTrigger = true; // Set to trigger to avoid physical collisions

        // Initialize energy to maximum
        currentEnergy = oxygenSystemStats.Energy;
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
        if (currentEnergy > 0)
        {
            currentEnergy -= oxygenSystemStats.DepletionRate * Time.deltaTime;
            currentEnergy = Mathf.Max(currentEnergy, 0); // Ensure energy does not go below 0
        }
    }

    private void UpdateRadius()
    {
        if (oxygenSystemStats != null)
        {
            // Calculate radius based on current energy as a percentage of max energy
            float radius = (oxygenSystemStats.Radius * currentEnergy) / oxygenSystemStats.Energy;

            // Update the visual circle and collider radius
            DrawCircle(radius);
            oxygenBubbleCollider.radius = radius;
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

    public void RefillEnergy(float amount)
    {
        // TODO: Maybe add a limit?
        currentEnergy += amount;
    }
}
