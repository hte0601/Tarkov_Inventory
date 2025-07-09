using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private ItemUI containableItemUI;  // 임시
    [SerializeField] private List<InventoryGridUI> gridList;
    private InventoryData data;

    private void Awake()
    {
        for (int i = 0; i < gridList.Count; i++)
        {
            gridList[i].Initialize(this, i);
        }
    }

    private void Start()
    {
        // 임시 코드
        if (containableItemUI != null
            && containableItemUI.Data is IContainableItem containableItem)
        {
            data = containableItem.InnerInventoryData;
        }
        else
        {
            RowColumn[] inventorySize = new RowColumn[gridList.Count];

            for (int i = 0; i < gridList.Count; i++)
            {
                inventorySize[i] = gridList[i].GridSize;
            }

            data = new(inventorySize);
        }

        InventoryUIManager.OpenUI(data, this);
        //
    }


    public void PlaceItemUIAtLocation(ItemLocation location, ItemUI item)
    {
        gridList[location.gridID].PlaceItemUIAtIndex(location.index, location.isRotated, item);
    }


    public void HandleItemDragBegin(int gridID, ItemUI itemUI)
    {
        ItemLocation fromLocation = new(data, gridID, itemUI.Data.GridIndex, itemUI.Data.IsRotated);
        ItemDragManager.instance.BeginItemDrag(fromLocation, itemUI);
    }

    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI itemUI)
    {
        bool canDrop;

        if (data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemData)
            && !ReferenceEquals(itemData, itemUI.Data))
        {
            if (itemData is IContainableItem containableItem)
            {
                canDrop = containableItem.InnerInventoryData.CanAddItemToInventory(itemUI.Data);
            }
            else
            {
                // 스택이 되는 아이템인 경우 따로 처리해야 함
                canDrop = false;
            }
        }
        else
        {
            ItemLocation dragLocation = new(data, gridID, dragIndex, itemUI.IsUIRotated);
            canDrop = data.CanAddItemAtLocation(dragLocation, itemUI.Data);
        }

        gridList[gridID].SetIndicator(dragIndex, itemUI.UISize, canDrop);
    }

    public void HandleItemDrop(int gridID, RowColumn mouseIndex, RowColumn dropIndex, ItemUI itemUI)
    {
        if (data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemData)
            && !ReferenceEquals(itemData, itemUI.Data))
        {
            if (itemData is IContainableItem containableItem
                && containableItem.InnerInventoryData.TryFindLocationToAddItem(itemUI.Data, out ItemLocation location))
            {
                ItemDragManager.instance.DropItem(location);

                Debug.LogFormat("({0}, {1}) 드래그 성공", mouseIndex.row, mouseIndex.col);
            }
            else
            {
                // 스택이 되는 아이템인 경우 따로 처리해야 함
                Debug.LogFormat("({0}, {1}) 드래그 실패", mouseIndex.row, mouseIndex.col);
            }
        }
        else
        {
            ItemLocation dropLocation = new(data, gridID, dropIndex, itemUI.IsUIRotated);

            if (data.CanAddItemAtLocation(dropLocation, itemUI.Data))
            {
                ItemDragManager.instance.DropItem(dropLocation);

                Debug.LogFormat("({0}, {1}) 드래그 성공", dropIndex.row, dropIndex.col);
            }
            else
            {
                Debug.LogFormat("({0}, {1}) 드래그 실패", dropIndex.row, dropIndex.col);
            }
        }
    }
}
