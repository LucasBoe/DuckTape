using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class SpatialInventoryItem : MonoBehaviour
{
    public Vector2Int Size;
    public SpatialInventoryArea Area;
    public CargoConfigBase Cargo;
    public GUID OriginStationID;
    
    [SerializeField, ReadOnly] private bool isHovered = false;

    private void OnMouseDrag()
    {
        Area?.TryPickUp(this);
        Debug.Log("Mouse Drag", gameObject);
    }
    public void SetPickedUp(bool isPickedUp)
    {
        GetComponent<BoxCollider2D>().enabled = !isPickedUp;
    }
    private void OnMouseEnter()
    {
        isHovered = true;
    }
    private void OnMouseExit()
    {
        isHovered = false;
    }
}