using UnityEngine;

[CreateAssetMenu(fileName = "OxygenSystemStats", menuName = "OxygenSystem/OxygenSystemStats")]
public class OxygenSystemStats : ScriptableObject
{
    [Range(0, 100)]
    [SerializeField] private float radius = 5f;

    [Range(0, 100)]
    [SerializeField] private float energy = 100f;

    [Tooltip("Energy depletion rate in percentage per second.")]
    [SerializeField] private float depletionRate = 1f; // Default 1% per second

    public float Radius { get { return radius; } set { radius = value; } }
    public float Energy { get { return energy; } set { energy = value; } }
    public float DepletionRate { get { return depletionRate; } set { depletionRate = value; } }
}
