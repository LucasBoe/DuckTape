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
        
        WagonSpawner.Instance.SpawnWagon(wagon, newSlot);
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
}