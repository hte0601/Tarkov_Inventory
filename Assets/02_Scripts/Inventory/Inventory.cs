using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
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
        RowColumn[] gridSizeArr = new RowColumn[gridList.Count];

        for (int i = 0; i < gridList.Count; i++)
        {
            gridSizeArr[i] = gridList[i].GridSize;
        }

        data = new(gridSizeArr);
        //
    }


    public void HandleItemDragBegin(int gridID, ItemUI item)
    {
        RemoveItemFromGrid(gridID, item);
        ItemDragManager.instance.BeginItemDrag(item);
    }

    public void HandleItemDragCancle(int gridID, ItemUI item)
    {
        ItemDragManager.instance.CancleItemDrag(gridList[gridID]);
        AddItemToGrid(gridID, item.Index, item);
    }

    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI item)
    {
        // data.GetItemAtIndex(gridID, mouseIndex)
        // 마우스 위치의 아이템이 내부 인벤토리를 가지는지 체크

        if (data.CanAddItemAtIndex(gridID, dragIndex, item))
        {
            gridList[gridID].SetIndicator(dragIndex, item.Size, true);
        }
        else
        {
            gridList[gridID].SetIndicator(dragIndex, item.Size, false);
        }
    }

    public void HandleItemDrop(int gridID, RowColumn dropIndex, ItemUI item)
    {
        if (data.CanAddItemAtIndex(gridID, dropIndex, item))
        {
            ItemDragManager.instance.DropItemToInventoryGrid(gridList[gridID]);
            AddItemToGrid(gridID, dropIndex, item);

            Debug.LogFormat("({0}, {1}) 드래그 성공", dropIndex.row, dropIndex.col);
        }
        else
        {
            Debug.LogFormat("({0}, {1}) 드래그 실패", dropIndex.row, dropIndex.col);
        }
    }


    private void AddItemToGrid(int gridID, RowColumn gridIndex, ItemUI item)
    {
        gridList[gridID].AddItemAtIndex(gridIndex, item);
        data.AddItemAtIndex(gridID, gridIndex, item);
    }

    private void RemoveItemFromGrid(int gridID, ItemUI item)
    {
        data.RemoveItem(gridID, item);
    }
}
