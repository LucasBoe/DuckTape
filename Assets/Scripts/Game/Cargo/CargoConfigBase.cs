using SS;
using UnityEngine;

public class CargoConfigBase : ContaineableScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int value;
    [SerializeField] private float weight;

    public Sprite Sprite => sprite;
    public int Value => value;
    public float Weight => weight;
}
