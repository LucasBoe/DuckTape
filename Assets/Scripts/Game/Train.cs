using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Train : MonoBehaviour
{
    private List<TrainWagonSlot> slots = new();
    public const float WAGON_DISTANCE = .5f;
    public void AppendFromConfig(WagonConfigBase wagon)
    {
        //find new slot x
        var x = CalculateTrainLength();

        //create new slot
        GameObject newSlotGameObject = new GameObject();
        newSlotGameObject.transform.parent = transform;
        newSlotGameObject.name = slots.Count.ToString();
        newSlotGameObject.transform.position = new Vector3(x, 0, 0);
        TrainWagonSlot newSlot = newSlotGameObject.AddComponent<TrainWagonSlot>();
        slots.Add(newSlot);
        
        var newWagonInstance = WagonSpawner.Instance.SpawnWagon(wagon, newSlot);
        if (newWagonInstance is Engine engine)
            DriveHandler.Instance.ModifyEngine(engine);
    }

    private float CalculateTrainLength()
    {
        float x = 0f;
        foreach (var slot in slots)
        {
            if (!slot.WagonInstance)
                continue;

            x -= slot.WagonInstance.Config.Length;
            x -= WAGON_DISTANCE;
        }

        return x;
    }
    public int CalculateTotalWeight()
    {
        int totalWeight = 0;
        foreach (var slot in slots)
            totalWeight += slot.WagonInstance.CalculateWeight();
        
        return totalWeight;
    }
}