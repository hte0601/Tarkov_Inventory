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


    public bool TryGetItemAtIndex(int gridID, RowColumn index, out ItemData item)
    {
        return gridDataArr[gridID].TryGetItemAtIndex(index, out item);
    }


    public bool CanAddItemAtLocation(ItemLocation location, ItemData item)
    {
        return gridDataArr[location.gridID].CanAddItemAtIndex(location.gridIndex, location.isItemRotated, item);
    }

    public bool AddItemAtLocation(ItemLocation location, ItemData item)
    {
        return gridDataArr[location.gridID].AddItemAtIndex(location.gridIndex, location.isItemRotated, item);
    }

    public void RemoveItem(int gridID, ItemData item)
    {
        gridDataArr[gridID].RemoveItem(item);
    }


    // 인벤토리에 아이템을 넣을 수 있는 자리가 있는지 확인
    public bool TryFindLocationToAddItem(ItemData item, out ItemLocation location)
    {
        for (int i = 0; i < GridCount; i++)
        {
            if (gridDataArr[i].TryFindIndexToAddItem(item, out RowColumn index, out bool isRotated))
            {
                location = new(this, i, index, isRotated);
                return true;
            }
        }

        location = new();
        return false;
    }

    public bool CanAddItemToInventory(ItemData item)
    {
        return TryFindLocationToAddItem(item, out _);
    }
}
