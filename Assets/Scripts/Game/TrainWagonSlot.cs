using UnityEngine;

public class TrainWagonSlot : MonoBehaviour
{
    public TrainWagonBase WagonInstance { get; private set; }
    public void Assign(TrainWagonBase instance)
    {
        WagonInstance = instance;
    }
}
