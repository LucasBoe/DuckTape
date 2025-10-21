using SS;
using UnityEngine;

public class CargoSpawner : Singleton<CargoSpawner>
{
    public Cargo Spawn(CargoConfigBase config)
    {
        GameObject newCargoObject = new GameObject("Cargo_" + config.name);
        SpriteRenderer newCargoRenderer = newCargoObject.AddComponent<SpriteRenderer>();
        Cargo newCargo = newCargoObject.AddComponent<Cargo>();
        newCargoRenderer.sortingLayerName = "InFrontOfUI";
        newCargoRenderer.sprite = config.Sprite;
        newCargo.OriginStationID = StatTracker.Instance.NumberOfStationsVisited;
        newCargo.Config = config;
        return newCargo;
    }
}