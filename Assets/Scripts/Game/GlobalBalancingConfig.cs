using UnityEngine;

[CreateAssetMenu]
public class GlobalBalancingConfig : ScriptableObject
{
    [SerializeField] public AnimationCurve ShakeOverTrainSpeedCurve;
    [SerializeField] public AnimationCurve SlopeAccelerationOverWeightCurve;
}