using SS;
using UnityEngine;

[SingletonSettings(SingletonLifetime.Persistant)]
public class WagonSpawner : Singleton<WagonSpawner>
{
    public void SpawnWagon(WagonConfigBase wagon, TrainWagonSlot newSlot)
    {
        var instance = Object.Instantiate(wagon.Prefab, newSlot.transform);
        newSlot.Assign(instance);
    }
}