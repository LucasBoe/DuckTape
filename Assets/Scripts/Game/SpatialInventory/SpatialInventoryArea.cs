using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

public class SpatialInventoryArea : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    private SpatialInventorySlot[,] slots;
    private List<SpatialInventorySlot> slotList = new();
    
    public const float SLOT_SIZE = 5/16f;
    public const float SLOT_GAP = 1/16f;
    private void Awake()
    {
        slots = new SpatialInventorySlot[size.x,size.y];
        //init empty
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var ss = (SLOT_SIZE + SLOT_GAP) / 2f * (Vector2)size;
                float xx = Mathf.Lerp(-ss.x, ss.x, (x + .5f) / size.x);
                float yy = Mathf.Lerp(-ss.y, ss.y, (y + .5f) / size.y);
                var slot = new SpatialInventorySlot(x,y, new Vector2(xx, yy));
                
                slots[x,y] = slot;
                slotList.Add(slot);
            }
        }
    }
    private void OnMouseEnter()
    {
        //hover enter
        SpatialInventoryHandler.Instance.NotifyEnter(this);
    }
    private void OnMouseExit()
    {
        //hover exit
        SpatialInventoryHandler.Instance.NotifyExit(this);
    }
    public bool TryGetAvailableSlotPlacements(Vector3 worldPosition, SpatialInventoryItem itemInHand,
        out SpatialPlacementInfo[] placement)
    {
        placement = SpatialUtil.GetAllValidPlacements(slots, transform, itemInHand.Size).ToArray();
        return placement.Length > 0;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            
        foreach (var slot in slotList)
        {
            Gizmos.color = slot.IsFree ? Color.white : Color.red;
            Gizmos.DrawWireCube( slot.GetWorldPosition(transform), Vector3.one * SLOT_SIZE);
        }
        }
        else
        {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var ss = (SLOT_SIZE + SLOT_GAP) / 2f * (Vector2)size;
                float xx = Mathf.Lerp(-ss.x, ss.x, (x + .5f) / size.x);
                float yy = Mathf.Lerp(-ss.y, ss.y, (y + .5f) / size.y);
                Gizmos.DrawWireCube( transform.position + new Vector3(xx, yy, 00), Vector3.one * SpatialInventoryArea.SLOT_SIZE);
            }
        }
        
        }
    }
    public bool TryAddItemFree(SpatialInventoryItem item)
    {
        var placements = SpatialUtil.GetAllValidPlacements(slots, transform, item.Size);
        if (placements.Count > 0)
        {
            AddItem(item, placements.First());
            return true;
        }
        
        return false;
    }
    public void AddItem(SpatialInventoryItem item, SpatialPlacementInfo currentPlacement)
    {
        item.transform.SetParent(transform);
        item.transform.position = currentPlacement.Position;
        item.Area = this;
        foreach (var slot in currentPlacement.AffectedSlots)
            slot.Item = item;
    }
    public void TryPickUp(SpatialInventoryItem item)
    {
        if (SpatialInventoryHandler.Instance.IsDragging)
            return;
        
        item.transform.SetParent(transform.root);
        item.Area = null;
        foreach (var slot in slotList)
            if (slot.Item == item)
                slot.Item = null;
        
        SpatialInventoryHandler.Instance.TryPickUp(item);
    }
}

public class SpatialInventorySlot
{
    public SpatialInventoryItem Item;
    public bool IsFree => !Item;
    public float X { get; private set; }
    public float Y { get; private set; }
    public Vector2 LocalPosition { get; private set; }
    public SpatialInventorySlot(float x, float y, Vector2 localPos)
    {
        X = x;
        Y = y;
        LocalPosition = localPos;
    }

    public Vector2 GetWorldPosition(Transform transform)
    {
        return (Vector2)transform.position + LocalPosition;
    }
}

public class SpatialPlacementInfo
{
    public Vector3 Position { get; set; }
    public SpatialInventorySlot[] AffectedSlots { get; set; }
}

public static class SpatialUtil 
{
    // Returns every valid placement for an item of `itemSize` (width x height in slots).
    public static List<SpatialPlacementInfo> GetAllValidPlacements(SpatialInventorySlot[,] grid, Transform transform, Vector2Int itemSize)
    {
        var results = new List<SpatialPlacementInfo>();
        
        if (itemSize.x <= 0 || itemSize.y <= 0)
            return results;

        int cols = grid.GetLength(0);
        int rows = grid.GetLength(1);
        
        if (itemSize.x > cols || itemSize.y > rows)
            return results;

        for (int c = 0; c <= cols - itemSize.x; c++)
        {
            for (int r = 0; r <= rows - itemSize.y; r++)
            {
                if (!RectIsFree(c, r, itemSize))
                    continue;

                // Collect covered slots.
                var covered = new SpatialInventorySlot[itemSize.x * itemSize.y];
                int k = 0;
                for (int dx = 0; dx < itemSize.x; dx++)
                    for (int dy = 0; dy < itemSize.y; dy++)
                        covered[k++] = grid[c + dx, r + dy];

                // World-space center of the covered rectangle.
                Vector2 tl = grid[c, r].GetWorldPosition(transform);
                Vector2 br = grid[c + itemSize.x - 1, r + itemSize.y - 1].GetWorldPosition(transform);
                Vector2 center = (tl + br) * 0.5f;

                results.Add(new SpatialPlacementInfo
                {
                    Position = new Vector3(center.x, center.y, transform.position.z),
                    AffectedSlots = covered
                });
            }
        }
        return results;

        bool RectIsFree(int c0, int r0, Vector2Int size)
        {
            for (int dx = 0; dx < size.x; dx++)
                for (int dy = 0; dy < size.y; dy++)
                    if (!grid[c0 + dx, r0 + dy].IsFree) return false;
            return true;
        }
    }
}