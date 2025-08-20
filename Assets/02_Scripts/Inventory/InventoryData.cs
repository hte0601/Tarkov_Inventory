using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    public RowColumn[] InventorySize { get; private set; }
    private readonly int gridCount;
    private readonly InventoryGridData[] gridDataArr;
    // public InventorySaveData SaveData { get; private set; }

    public InventoryData(RowColumn[] inventorySize)
    {
        InventorySize = inventorySize;
        gridCount = inventorySize.Length;

        gridDataArr = new InventoryGridData[gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            gridDataArr[i] = new InventoryGridData(i, inventorySize[i]);
        }

        // SaveData = new();
    }


    public bool TryGetItemAtIndex(int gridID, RowColumn gridIndex, out ItemData item)
    {
        return gridDataArr[gridID].TryGetItemAtIndex(gridIndex, out item);
    }


    public bool CanAddItemAtLocation(ItemData item, ItemLocation addLocation)
    {
        return gridDataArr[addLocation.gridID].CanAddItemAtIndex(item, addLocation.gridIndex, addLocation.isItemRotated);
    }

    public void AddItemAtLocation(ItemData item, ItemLocation addLocation)
    {
        gridDataArr[addLocation.gridID].AddItemAtIndex(item, addLocation.gridIndex, addLocation.isItemRotated);
    }

    public bool CanRemoveItem(ItemData item)
    {
        return gridDataArr[item.gridID].CanRemoveItem(item);
    }

    public void RemoveItem(ItemData item)
    {
        gridDataArr[item.gridID].RemoveItem(item);
    }


    // 인벤토리에 아이템을 넣을 수 있는 자리가 있는지 확인
    public bool TryFindLocationToAddItem(ItemData item, out ItemLocation addLocation)
    {
        for (int i = 0; i < gridCount; i++)
        {
            if (gridDataArr[i].TryFindIndexToAddItem(item, out RowColumn gridIndex, out bool isItemRotated))
            {
                addLocation = new ItemLocation(this, i, gridIndex, isItemRotated);
                return true;
            }
        }

        addLocation = new ItemLocation();
        return false;
    }

    public bool CanAddItemToInventory(ItemData item)
    {
        return TryFindLocationToAddItem(item, out _);
    }
}
