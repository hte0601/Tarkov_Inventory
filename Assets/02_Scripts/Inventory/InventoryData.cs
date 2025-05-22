using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    private InventoryGridData[] gridDataArr;
    private int gridNumber;

    public InventoryData(RowColumn[] gridSizeArr)
    {
        gridNumber = gridSizeArr.Length;
        gridDataArr = new InventoryGridData[gridNumber];

        for (int i = 0; i < gridNumber; i++)
        {
            gridDataArr[i] = new InventoryGridData(gridSizeArr[i]);
        }
    }


    public bool CanAddItemAtIndex(int gridID, RowColumn gridIndex, ItemUI item)
    {
        return gridDataArr[gridID].CanAddItemAtIndex(gridIndex, item);
    }

    public bool AddItemAtIndex(int gridID, RowColumn gridIndex, ItemUI item)
    {
        return gridDataArr[gridID].AddItemAtIndex(gridIndex, item);
    }

    public void RemoveItem(int gridID, ItemUI item)
    {
        gridDataArr[gridID].RemoveItem(item);
    }


    // 아이템이 들어갈 수 있는 자리가 있는지
    // private bool TryFind(ItemUI item, out int gridID, out RowColumn index, out bool isRotated)
    // {
    //     for (int i = 0; i < gridDataArr.Length; i++)
    //     {
    //         if (gridDataArr[i].CanAddItemToGrid(item, out index, out isRotated))
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    // public bool CanAddItemToInventory(ItemUI item)
    // {
    //     return TryFind(item, out _, out _, out _);
    // }


    // public bool AddItemToInventory()
    // {
    //     return false;
    // }
}
