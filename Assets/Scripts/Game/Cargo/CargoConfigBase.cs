using SS;
using UnityEngine;

public class CargoConfigBase : ContaineableScriptableObject
{
    public enum CargoType {Small, Medium, Large}

    [SerializeField] private CargoType type;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int value;
    [SerializeField] private float weight;

    public CargoType Type => type;
    public Sprite Sprite => sprite;
    public int Value => value;
    public float Weight => weight;
}
