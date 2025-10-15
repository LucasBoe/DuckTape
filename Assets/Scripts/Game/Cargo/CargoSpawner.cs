using SS;
using UnityEngine;

public class CargoSpawner : Singleton<CargoSpawner>
{
    public Cargo Spawn(CargoConfigBase config)
    {
        GameObject newCargo = new GameObject();
        newCargo.name = "Cargo_" + config.name;
        newCargo.AddComponent<Cargo>().CargoConfig = config;
        newCargo.AddComponent<SpriteRenderer>().sprite = config.Sprite;
        return newCargo.GetComponent<Cargo>();
    }

    public void SpawnAtSlot(CargoConfigBase config , CargoSlot slot)
    {
        slot.Assign(Spawn(config));
        slot.CargoInstance.transform.SetParent(slot.transform);
        slot.CargoInstance.gameObject.transform.localPosition = Vector3.zero;
    }
}