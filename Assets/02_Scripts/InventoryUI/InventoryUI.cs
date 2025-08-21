using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    public event Action<IInventoryUI, ItemUI> OnItemDragBegin;
    public event Action<IInventoryUI, ItemUI> OnItemDragEnd;
    public event Action<IInventoryUI, ItemUI, ItemLocation> OnItemDropOnInventoryGrid;
    public event Action<IInventoryUI, ItemUI, IContainableItemData> OnItemDropOnContainableItem;

    [SerializeField] private List<InventoryGridUI> gridList;
    public RowColumn[] inventorySize;
    private Dictionary<RowColumn, ItemUI>[] itemUIList;

    public InventoryData Data { get; private set; }

    private void Awake()
    {
        int gridCount = gridList.Count;
        inventorySize = new RowColumn[gridCount];
        itemUIList = new Dictionary<RowColumn, ItemUI>[gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            gridList[i].Initialize(this, i);
            inventorySize[i] = gridList[i].GridSize;
            itemUIList[i] = new Dictionary<RowColumn, ItemUI>();
        }
    }


    public void SetupUI(InventoryData inventoryData)
    {
        Data = inventoryData;

        // 인벤토리 UI에 ItemUI 오브젝트 추가
    }

    public void ResetUI()
    {
        // 모든 ItemUI 오브젝트를 풀에 반환

        Data = null;
    }


    public bool AddItemUI(ItemUI itemUI)
    {
        ItemData itemData = itemUI.Data;

        if (itemUIList[itemData.gridID].TryAdd(itemData.gridIndex, itemUI))
        {
            gridList[itemData.gridID].AddItemUIAtIndex(itemUI, itemData.gridIndex, itemData.IsItemRotated);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveItemUI(int gridID, RowColumn gridIndex, out ItemUI itemUI)
    {
        if (itemUIList[gridID].Remove(gridIndex, out itemUI))
        {
            itemUI.parentGridUI = null;

            return true;
        }
        else
        {
            return false;
        }
    }


    // InventoryGridUI의 드래그 드랍 이벤트에 대한 핸들러
    public void HandleItemDragBegin(ItemUI itemUI, int gridID)
    {
        OnItemDragBegin?.Invoke(this, itemUI);
    }

    public void HandleItemDragEnd(ItemUI itemUI, int gridID)
    {
        OnItemDragEnd?.Invoke(this, itemUI);
    }

    public void HandleItemDrop(ItemDragContext dragContext, int gridID, RowColumn mouseIndex, RowColumn dropIndex)
    {
        ItemUI droppedItemUI = dragContext.DraggingItemUI;

        if (Data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemInSlot)
            && !ReferenceEquals(itemInSlot, dragContext.ItemData))
        {
            if (itemInSlot is IContainableItemData containableItem)
            {
                OnItemDropOnContainableItem?.Invoke(this, droppedItemUI, containableItem);
                return;
            }
            // else if (itemInSlot is IStackableItemData stackableItem) { }
        }

        ItemLocation dropLocation = new(Data, gridID, dropIndex, dragContext.IsItemUIRotated);
        OnItemDropOnInventoryGrid?.Invoke(this, droppedItemUI, dropLocation);
    }

    public void HandleItemDragOver(ItemDragContext dragContext, int gridID, RowColumn mouseIndex, RowColumn dragOverIndex)
    {
        bool? canDrop = null;

        if (Data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemInSlot)
            && !ReferenceEquals(itemInSlot, dragContext.ItemData))
        {
            if (itemInSlot is IContainableItemData containableItem)
            {
                canDrop = containableItem.InnerInventoryData.CanAddItemToInventory(dragContext.ItemData);
            }
            // else if (itemInSlot is IStackableItemData stackableItem) { }
        }

        if (!canDrop.HasValue)
        {
            ItemLocation dragOverLocation = new(Data, gridID, dragOverIndex, dragContext.IsItemUIRotated);
            canDrop = Data.CanAddItemAtLocation(dragContext.ItemData, dragOverLocation);
        }

        gridList[gridID].SetIndicator(dragOverIndex, dragContext.ItemUISize, canDrop.Value);
    }
}
