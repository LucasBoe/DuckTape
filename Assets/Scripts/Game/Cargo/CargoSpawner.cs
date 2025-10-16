using SS;
using UnityEngine;

public class CargoSpawner : Singleton<CargoSpawner>
{
    public Cargo Spawn(CargoConfigBase config)
    {
        GameObject newCargoObject = new GameObject("Cargo_" + config.name);
        newCargoObject.AddComponent<SpriteRenderer>().sprite = config.Sprite;
        Cargo newCargo = newCargoObject.AddComponent<Cargo>();
        newCargo.Config = config;
        return newCargo;
    }
    public void SpawnAtSlot(CargoConfigBase config , CargoSlot slot)
    {
        slot.Assign(Spawn(config));
    }
}