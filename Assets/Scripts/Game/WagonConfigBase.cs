using SS;
using UnityEngine;

public class WagonConfigBase : ContaineableScriptableObject
{
    [SerializeField] private TrainWagonBase prefab;
    [SerializeField] private float length;
    public int EmptyWeight = 1000;
    public float Length => length;
    public TrainWagonBase Prefab => prefab;
}