using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    public RowColumn[] InventorySize { get; private set; }
    public int GridCount { get; private set; }
    private InventoryGridData[] gridDataArr;
    public InventorySaveData SaveData { get; private set; }

    public InventoryData(RowColumn[] inventorySize)
    {
        InventorySize = inventorySize;
        GridCount = inventorySize.Length;

        gridDataArr = new InventoryGridData[GridCount];

        for (int i = 0; i < GridCount; i++)
        {
            gridDataArr[i] = new(inventorySize[i]);
        }

        SaveData = new();
    }


    public bool CanAddItemAtLocation(ItemLocation location, ItemData item)
    {
        return gridDataArr[location.gridID].CanAddItemAtIndex(location.index, location.isRotated, item);
    }

    public bool AddItemAtLocation(ItemLocation location, ItemData item)
    {
        return gridDataArr[location.gridID].AddItemAtIndex(location.index, location.isRotated, item);
    }

    public void RemoveItem(int gridID, ItemData item)
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
