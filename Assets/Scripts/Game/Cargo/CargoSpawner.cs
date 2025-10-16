using SS;
using UnityEngine;

public class CargoSpawner : Singleton<CargoSpawner>
{
    public Cargo Spawn(CargoConfigBase config)
    {
        GameObject newCargoObject = new GameObject("Cargo_" + config.name);
        newCargoObject.AddComponent<SpriteRenderer>().sprite = config.Sprite;
        Cargo newCargo = newCargoObject.AddComponent<Cargo>();
        newCargo.OriginStationID = StatTracker.Instance.NumberOfStationsVisited;
        newCargo.Config = config;
        return newCargo;
    }
}