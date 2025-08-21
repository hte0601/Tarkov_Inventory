using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController
{
    public static InventoryUIController instance = new();

    private readonly Dictionary<InventoryData, IInventoryUI> openedInventoryUIDict = new();

    private InventoryUIController()
    {
        InventoryDataManager inventoryDataManager = InventoryDataManager.instance;

        inventoryDataManager.OnItemDataAdded += HandleItemDataAdded;
        inventoryDataManager.OnItemDataRemoved += HandleItemDataRemoved;
        inventoryDataManager.OnItemDataMoved += HandleItemDataMoved;
    }


    // 임시 메소드
    public bool OpenUI(InventoryData inventoryData, IInventoryUI inventoryUI)
    {
        if (openedInventoryUIDict.TryAdd(inventoryData, inventoryUI))
        {
            inventoryUI.SetupUI(inventoryData);

            inventoryUI.OnItemDragBegin += HandleItemDragBegin;
            inventoryUI.OnItemDragEnd += HandleItemDragEnd;
            inventoryUI.OnItemDropOnInventoryGrid += HandleItemDropOnInventoryGrid;
            inventoryUI.OnItemDropOnContainableItem += HandleItemDropOnContainableItem;

            return true;
        }
        else
        {
            return false;
        }
    }

    // private bool OpenUI(InventoryData inventoryData) { }
    // private bool CloseUI(InventoryData inventoryData) { }


    private bool TryGetUI(InventoryData inventoryData, out IInventoryUI inventoryUI)
    {
        return openedInventoryUIDict.TryGetValue(inventoryData, out inventoryUI);
    }


    // InventoryUI의 드래그 드랍 이벤트에 대한 핸들러
    private void HandleItemDragBegin(IInventoryUI inventoryUI, ItemUI itemUI)
    {
        if (!ItemDragManager.instance.dragContext.IsDragging)
        {
            ItemLocation fromLocation = new(inventoryUI.Data, itemUI.Data.gridID, itemUI.Data.gridIndex, itemUI.Data.IsItemRotated);
            ItemDragManager.instance.BeginItemDrag(itemUI, fromLocation);
        }
    }

    private void HandleItemDragEnd(IInventoryUI inventoryUI, ItemUI itemUI)
    {
        ItemDragContext dragContext = ItemDragManager.instance.dragContext;

        // drop 처리가 안 된 경우
        if (dragContext.IsDragging && ReferenceEquals(itemUI, dragContext.DraggingItemUI))
        {
            ItemDragManager.instance.CancelItemDrag();
        }
    }

    private void HandleItemDropOnInventoryGrid(IInventoryUI inventoryUI, ItemUI droppedItemUI, ItemLocation dropLocation)
    {
        ItemDragContext dragContext = ItemDragManager.instance.dragContext;

        if (dragContext.IsDragging && ReferenceEquals(droppedItemUI, dragContext.DraggingItemUI))
        {
            ItemDragManager.instance.EndItemDrag(dropLocation);
        }
    }

    private void HandleItemDropOnContainableItem(IInventoryUI inventoryUI, ItemUI droppedItemUI, IContainableItemData containableItem)
    {
        ItemDragContext dragContext = ItemDragManager.instance.dragContext;
        ItemData itemData = droppedItemUI.Data;

        if (dragContext.IsDragging && ReferenceEquals(droppedItemUI, dragContext.DraggingItemUI)
            && containableItem.InnerInventoryData.TryFindLocationToAddItem(itemData, out ItemLocation dropLocation))
        {
            ItemDragManager.instance.EndItemDrag(dropLocation);
        }
    }


    // InventoryDataManager의 데이터 변경 이벤트에 대한 핸들러
    private void HandleItemDataAdded(ItemData itemData, ItemLocation addLocation)
    {
        if (TryGetUI(addLocation.inventoryData, out IInventoryUI inventoryUI))
        {
            ItemUI itemUI = ItemUIPool.instance.GetItemUI(itemData);
            inventoryUI.AddItemUI(itemUI);
        }
    }

    private void HandleItemDataRemoved(ItemLocation removeLocation)
    {
        if (TryGetUI(removeLocation.inventoryData, out IInventoryUI inventoryUI))
        {
            inventoryUI.RemoveItemUI(removeLocation.gridID, removeLocation.gridIndex, out ItemUI itemUI);
            ItemUIPool.instance.ReleaseItemUI(itemUI);
        }
    }

    private void HandleItemDataMoved(ItemData itemData, ItemLocation fromLocation, ItemLocation toLocation)
    {
        ItemUI itemUI;
        bool isFromInventoryUIOpen = TryGetUI(fromLocation.inventoryData, out IInventoryUI fromInventoryUI);
        bool isToInventoryUIOpen = TryGetUI(toLocation.inventoryData, out IInventoryUI toInventoryUI);

        if (isFromInventoryUIOpen && isToInventoryUIOpen)
        {
            fromInventoryUI.RemoveItemUI(fromLocation.gridID, fromLocation.gridIndex, out itemUI);
            toInventoryUI.AddItemUI(itemUI);
        }
        else if (isFromInventoryUIOpen)
        {
            fromInventoryUI.RemoveItemUI(fromLocation.gridID, fromLocation.gridIndex, out itemUI);
            ItemUIPool.instance.ReleaseItemUI(itemUI);
        }
        else if (isToInventoryUIOpen)
        {
            itemUI = ItemUIPool.instance.GetItemUI(itemData);
            toInventoryUI.AddItemUI(itemUI);
        }
    }
}
