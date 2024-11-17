using Interfaces;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(CircleCollider2D))]
public class OxygenSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private OxygenSystemStats oxygenSystemStats;

    private LineRenderer lineRenderer;
    private CircleCollider2D oxygenBubbleCollider;
    private int segments = 50; // Defines the smoothness of the circle
    [SerializeField] private float currentEnergy;
    private float _initialEnergy;
    public float CurrentEnergy { get => currentEnergy; set => currentEnergy = value; }
    [SerializeField] GameObject circleVisual;

    // This is a light blue color.
    [SerializeField, ColorUsage(true, true)]
    private Color borderColor = new Color(49/255f,227/255f,250/255f,1f);

    public float CurrentRadius { get => oxygenBubbleCollider.radius; }

    private void Awake()
    {
        // Set up LineRenderer component for the border
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the material to avoid pink color
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = borderColor;
        lineRenderer.endColor = borderColor;

        // Initialize and configure the CircleCollider2D for the oxygen bubble
        oxygenBubbleCollider = GetComponent<CircleCollider2D>();
        oxygenBubbleCollider.isTrigger = true; // Set to trigger to avoid physical collisions

        // Initialize energy to maximum
        currentEnergy = oxygenSystemStats.Energy;
        _initialEnergy = oxygenSystemStats.Energy;
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
            circleVisual.transform.localScale = new Vector3(radius, radius, .5f)*2;

            // Update the visual circle and collider radius
            DrawCircle(radius);
            if (currentEnergy <= 0 && oxygenBubbleCollider.enabled == true) oxygenBubbleCollider.enabled = false;
            else oxygenBubbleCollider.enabled = true;
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
        currentEnergy += amount;

        if(currentEnergy > oxygenSystemStats.Energy) 
        {
            currentEnergy = oxygenSystemStats.Energy;
        }
    }

    public float GetCurrentEnergy() 
    {
        return currentEnergy;
    }

    public float GetMaxEnergy() 
    {
        return _initialEnergy;
    }

    public void TakeDamage(float damage)
    {
        currentEnergy -= oxygenSystemStats.DepletionRate + damage / 2;
    }
}
