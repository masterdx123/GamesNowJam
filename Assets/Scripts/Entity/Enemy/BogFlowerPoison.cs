using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BogFlowerPoison : MonoBehaviour
{

    [SerializeField] private int damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float radiusSpeed;
    private LineRenderer lineRenderer;
    private CircleCollider2D damageCollider;
    private int segments = 50;
    private float currentRadius;
    private void Awake()
    {
        currentRadius = 0.1f;

        // Set up LineRenderer component for the border
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Initialize and configure the CircleCollider2D for the oxygen bubble
        damageCollider = GetComponent<CircleCollider2D>();
        damageCollider.isTrigger = true; // Set to trigger to avoid physical collisions
    }

    private void Start()
    {
        
    }

   private void Update()
    {
        UpdateRadius();
    }

    private void UpdateRadius()
    {
        //only increase size if it less than the max radius
        if (currentRadius < damageRadius)
        {
            currentRadius += Time.deltaTime * radiusSpeed;

            // Update the visual circle and collider radius
            DrawCircle(currentRadius);
            damageCollider.radius = currentRadius;
        }
        else { DrawCircle(currentRadius); }

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

    public float GetDamage()
    {
        return damage;
    }
}
