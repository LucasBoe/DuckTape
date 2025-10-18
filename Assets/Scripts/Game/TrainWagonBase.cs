using System;
using UnityEngine;

public class TrainWagonBase : MonoBehaviour
{
    [SerializeField] private WagonConfigBase wagonConfig;
    public WagonConfigBase Config => wagonConfig;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(-wagonConfig.Length, 10), new Vector3(-wagonConfig.Length, -10));
    }
    public virtual int CalculateWeight() => wagonConfig.EmptyWeight;
}