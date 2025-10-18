using SS;
using UnityEngine;

[SingletonSettings(SingletonLifetime.Persistant)]
public class WagonSpawner : Singleton<WagonSpawner>
{
    public TrainWagonBase SpawnWagon(WagonConfigBase wagon, TrainWagonSlot slot)
    {
        var instance = Object.Instantiate(wagon.Prefab, slot.transform);
        slot.Assign(instance);
        return instance;
    }
}