using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CargoSlot : MonoBehaviour
{
    public Cargo CargoInstance;

    public void Assign(Cargo instance)
    {
        CargoInstance = instance;
    }

    public void AssignFromConfig(CargoConfigBase config)
    {
        Assign(CargoSpawner.Instance.Spawn(config));
    }
}