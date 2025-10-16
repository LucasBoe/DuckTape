using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CargoSlot : MonoBehaviour
{
    public Cargo CargoInstance;
    public bool ContainsCargo => CargoInstance;

    public void Assign(Cargo instance)
    {
        CargoInstance = instance;
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
    }

    public void AssignFromConfig(CargoConfigBase config)
    {
        Assign(CargoSpawner.Instance.Spawn(config));
    }

    public Cargo ExtractCargo()
    {
        var cargo = CargoInstance;
        CargoInstance = null;
        return cargo;
    }
}