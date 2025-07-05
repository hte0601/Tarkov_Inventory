using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IInventoryUI
{
    [SerializeField] private List<InventoryGridUI> gridList;
    private InventoryData data;

    private void Awake()
    {
        for (int i = 0; i < gridList.Count; i++)
        {
            gridList[i].Initialize(this, i);
        }

        // 임시 코드
        RowColumn[] inventorySize = new RowColumn[gridList.Count];

        for (int i = 0; i < gridList.Count; i++)
        {
            inventorySize[i] = gridList[i].GridSize;
        }

        data = new(inventorySize);
        InventoryUIManager.OpenUI(data, this);
        //
    }


    public void HandleItemDragBegin(int gridID, ItemUI itemUI)
    {
        ItemLocation fromLocation = new(data, gridID, itemUI.Data.GridIndex, itemUI.Data.IsRotated);
        ItemDragManager.instance.BeginItemDrag(fromLocation, itemUI);
    }

    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI itemUI)
    {
        // data.GetItemAtIndex(gridID, mouseIndex)
        // 마우스 위치의 아이템이 내부 인벤토리를 가지는지 체크

        ItemLocation dragLocation = new(data, gridID, dragIndex, itemUI.IsUIRotated);
        bool canDrop = data.CanAddItemAtLocation(dragLocation, itemUI.Data);

        gridList[gridID].SetIndicator(dragIndex, itemUI.UISize, canDrop);
    }

    public void HandleItemDrop(int gridID, RowColumn mouseIndex, RowColumn dropIndex, ItemUI itemUI)
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


    public void PlaceItemUIAtLocation(ItemLocation location, ItemUI item)
    {
        gridList[location.gridID].PlaceItemUIAtIndex(location.index, location.isRotated, item);
    }
}
