using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    public event Action<IInventoryUI, ItemUI> OnItemDoubleClick;
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
            gridList[i].Initialize(i);
            gridList[i].OnItemDoubleClick += HandleItemDoubleClick;
            gridList[i].OnItemDragBegin += HandleItemDragBegin;
            gridList[i].OnItemDragEnd += HandleItemDragEnd;
            gridList[i].OnItemDrop += HandleItemDrop;
            gridList[i].OnItemDragOver += HandleItemDragOver;

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
            gridList[gridID].RemoveItemUI(itemUI);

            return true;
        }
        else
        {
            return false;
        }
    }


    // InventoryGridUI의 아이템 관련 이벤트에 대한 핸들러
    private void HandleItemDoubleClick(int gridID, ItemUI itemUI)
    {
        OnItemDoubleClick?.Invoke(this, itemUI);
    }

    private void HandleItemDragBegin(int gridID, ItemUI itemUI)
    {
        OnItemDragBegin?.Invoke(this, itemUI);
    }

    private void HandleItemDragEnd(int gridID, ItemUI itemUI)
    {
        OnItemDragEnd?.Invoke(this, itemUI);
    }

    private void HandleItemDrop(int gridID, ItemDragContext dragContext, RowColumn mouseIndex, RowColumn dropIndex)
    {
        ItemUI droppedItemUI = dragContext.DraggingItemUI;

        if (Data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemInSlot)
            && !ReferenceEquals(itemInSlot, droppedItemUI.Data))
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

    private void HandleItemDragOver(int gridID, ItemDragContext dragContext, RowColumn mouseIndex, RowColumn dragOverIndex)
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
