using System;
using UnityEditor;
using UnityEngine;

public class SpatialInventoryItem : MonoBehaviour
{
    public Vector2Int Size;
    public SpatialInventoryArea Area;
    public CargoConfigBase Cargo;
    public GUID OriginStationID;

    private void OnMouseDrag()
    {
        Area?.TryPickUp(this);
    }
    public void SetPickedUp(bool isPickedUp)
    {
        GetComponent<BoxCollider2D>().enabled = !isPickedUp;
    }
}