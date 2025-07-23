using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private List<InventoryGridUI> gridList;
    public RowColumn[] inventorySize;

    public InventoryData Data { get; private set; }

    private void Awake()
    {
        int gridCount = gridList.Count;
        inventorySize = new RowColumn[gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            gridList[i].Initialize(this, i);
            inventorySize[i] = gridList[i].GridSize;
        }
    }


    public void SetupUI(InventoryData inventoryData)
    {
        Data = inventoryData;

        // 인벤토리 UI에 ItemUI 오브젝트 추가

        InventoryUIManager.OpenUI(Data, this);
    }

    public void ResetUI()
    {
        InventoryUIManager.CloseUI(Data);

        // 모든 ItemUI 오브젝트를 풀에 반환

        Data = null;
    }


    public void PlaceItemUIAtLocation(ItemLocation location, ItemUI item)
    {
        gridList[location.gridID].PlaceItemUIAtIndex(location.gridIndex, location.isItemRotated, item);
    }


    public void HandleItemDragBegin(int gridID, ItemUI itemUI)
    {
        ItemLocation fromLocation = new(Data, gridID, itemUI.Data.GridIndex, itemUI.Data.IsRotated);
        ItemDragManager.instance.BeginItemDrag(fromLocation, itemUI);
    }

    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI itemUI)
    {
        bool canDrop;

        if (Data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemData)
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
            ItemLocation dragLocation = new(Data, gridID, dragIndex, itemUI.IsUIRotated);
            canDrop = Data.CanAddItemAtLocation(dragLocation, itemUI.Data);
        }

        gridList[gridID].SetIndicator(dragIndex, itemUI.UISize, canDrop);
    }

    public void HandleItemDrop(int gridID, RowColumn mouseIndex, RowColumn dropIndex, ItemUI itemUI)
    {
        if (Data.TryGetItemAtIndex(gridID, mouseIndex, out ItemData itemData)
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
            ItemLocation dropLocation = new(Data, gridID, dropIndex, itemUI.IsUIRotated);

            if (Data.CanAddItemAtLocation(dropLocation, itemUI.Data))
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
