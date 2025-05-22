using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RowColumn
{
    public int row;
    public int col;
}


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


    public void HandleItemBeginDrag(int gridID, RowColumn itemIndex, ItemUI item)
    {
        RemoveItemFromGrid(gridID, item);
        ItemDragManager.instance.BeginItemDrag(item);
    }

    public void HandleItemCancleDrag(int gridID, RowColumn itemIndex, ItemUI item)
    {
        ItemDragManager.instance.CancleItemDrag(gridList[gridID]);
        AddItemToGrid(gridID, itemIndex, item);
    }

    public void HandleItemDragOver(int gridID, RowColumn eventIndex, ItemUI item)
    {
        //
    }

    public void HandleItemDropOn(int gridID, RowColumn eventIndex, ItemUI item)
    {
        if (data.CanAddItemAtIndex(gridID, eventIndex, item))
        {
            ItemDragManager.instance.DropItemToInventoryGrid(gridList[gridID]);
            AddItemToGrid(gridID, eventIndex, item);

            Debug.LogFormat("({0}, {1}) 드래그 성공", eventIndex.row, eventIndex.col);
        }
        else
        {
            Debug.LogFormat("({0}, {1}) 드래그 실패", eventIndex.row, eventIndex.col);
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
