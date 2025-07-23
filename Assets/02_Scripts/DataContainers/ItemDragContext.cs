using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragContext
{
    public bool IsDragging { get; private set; }
    public ItemLocation FromLocation { get; private set; }
    public ItemUI DraggingItemUI { get; private set; }
    public bool IsItemUIRotated { get; private set; }

    public ItemData ItemData { get => DraggingItemUI.Data; }
    public RowColumn ItemUISize { get => DraggingItemUI.Data.GetItemSize(IsItemUIRotated); }
    public Vector2 TopLeftCellOffset { get => DraggingItemUI.GetTopLeftCellOffset(IsItemUIRotated); }

    public void BeginItemDrag(ItemLocation fromLocation, ItemUI draggingItemUI)
    {
        IsDragging = true;
        FromLocation = fromLocation;
        DraggingItemUI = draggingItemUI;
        IsItemUIRotated = fromLocation.isItemRotated;
    }

    public void EndItemDrag()
    {
        IsDragging = false;
        FromLocation = new();
        DraggingItemUI = null;
        IsItemUIRotated = false;
    }

    public void RotateItemUI()
    {
        IsItemUIRotated = !IsItemUIRotated;
    }
}
