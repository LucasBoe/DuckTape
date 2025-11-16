using System.Linq;
using NaughtyAttributes;
using SS;
using Unity.VisualScripting;
using UnityEngine;

[SingletonSettings(SingletonLifetime.Scene, true, true)]
public class SpatialInventoryHandler : SingletonBehaviour<SpatialInventoryHandler>
{
    [SerializeField, ReadOnly] private SpatialInventoryItem itemInHand;
    [SerializeField, ReadOnly] private SpatialInventoryArea currentlyHoveredArea;
    private SpatialPlacementInfo currentPlacement;
    [ShowNativeProperty] public bool IsDragging => itemInHand;
    public SpatialInventoryItem CurrentItem => IsDragging ? itemInHand : null;
    public Event<SpatialInventoryItem> PickUpEvent = new(), DropEvent = new();

    private void Update()
    {
        if (!itemInHand)
            return;
        
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        currentPlacement = null;
        
        if (currentlyHoveredArea 
            && currentlyHoveredArea.TryGetAvailableSlotPlacements(mouseWorldPosition, itemInHand, out var placements))
        {
            currentPlacement = placements.OrderBy(p => Vector2.Distance(p.Position, mouseWorldPosition)).First();
        }

        itemInHand.transform.position = currentPlacement?.Position ?? mouseWorldPosition;

        if (currentPlacement != null && Input.GetMouseButtonUp(0))
        {
            currentlyHoveredArea.AddItem(itemInHand, currentPlacement);
            itemInHand.SetPickedUp(false);
            DropEvent?.Invoke(itemInHand);
            itemInHand = null;
        }
    }
    public void NotifyEnter(SpatialInventoryArea spatialInventoryArea)
    {
        currentlyHoveredArea = spatialInventoryArea;
    }
    public void NotifyExit(SpatialInventoryArea spatialInventoryArea)
    {
        if (currentlyHoveredArea == spatialInventoryArea)
            currentlyHoveredArea = null;
    }
    public SpatialInventoryItem Create(CargoConfigBase cargoConfig)
    {
        GameObject itemGo = new GameObject($"{cargoConfig.name}");
        var item = itemGo.AddComponent<SpatialInventoryItem>();
        var size = ((SpatialInventoryArea.SLOT_SIZE + SpatialInventoryArea.SLOT_GAP) * (Vector2)cargoConfig.Size - Vector2.one * SpatialInventoryArea.SLOT_GAP);
        item.AddComponent<SpriteRenderer>().sprite = cargoConfig.Sprite;
        item.AddComponent<BoxCollider2D>().size = size;
        item.OriginStationID = StationHandler.Instance.CurrentStation.ID;
        item.Size = cargoConfig.Size;
        item.Cargo = cargoConfig;
        return item;
    }
    private void OnDrawGizmos()
    {
        if (!itemInHand)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(itemInHand.transform.position, (Vector2)itemInHand.Size * SpatialInventoryArea.SLOT_SIZE);
    }
    public void TryPickUp(SpatialInventoryItem item)
    {
        if (itemInHand)
            return;

        item.SetPickedUp(true);
        itemInHand = item;
        PickUpEvent?.Invoke(itemInHand);
    }
    public void SellCurrentTo(WorldMapNode station)
    {
        MoneyHandler.Instance.ChangeMoney(itemInHand.Cargo.Value, itemInHand.transform.position);
        DropEvent?.Invoke(itemInHand);
        Destroy(itemInHand.gameObject);
        itemInHand = null;
    }
}