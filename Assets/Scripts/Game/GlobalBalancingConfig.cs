using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class GlobalBalancingConfig : ScriptableObject
{
    [SerializeField] public AnimationCurve ShakeOverTrainSpeedCurve;
    [SerializeField] public AnimationCurve SlopeAccelerationOverWeightCurve;

    [SerializeField, BoxGroup("Maintenance")] public float RepairCostByMissingHealth;
    [SerializeField, BoxGroup("Maintenance")] public int CoalCost;
    [SerializeField, BoxGroup("Maintenance")] public int SandCost;
}