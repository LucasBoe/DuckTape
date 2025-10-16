using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CargoSlot : MonoBehaviour
{
    public Cargo CargoInstance;
    public bool ContainsCargo => CargoInstance;

    public virtual bool TryAssign(Cargo instance)
    {
        if (ContainsCargo)
            return false;
            
        CargoInstance = instance;
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        return true;
    }

    public void AssignFromConfig(CargoConfigBase config)
    {
        TryAssign(CargoSpawner.Instance.Spawn(config));
    }

    public Cargo ExtractCargo()
    {
        var cargo = CargoInstance;
        CargoInstance = null;
        return cargo;
    }
}