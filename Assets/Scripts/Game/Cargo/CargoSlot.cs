using System.Collections;
using UnityEngine;

public class CargoSlot : MonoBehaviour
{
    public Cargo CargoInstance { get; private set; }

    public void Assign(Cargo instance)
    {
        CargoInstance = instance;
    }
}
