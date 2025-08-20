using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDataManager
{
    public static InventoryDataManager instance = new();

    public event Action<ItemData, ItemLocation> OnItemDataAdded;
    public event Action<ItemLocation> OnItemDataRemoved;
    public event Action<ItemData, ItemLocation, ItemLocation> OnItemDataMoved;


    public bool AddItemData(ItemData itemData, ItemLocation addLocation)
    {
        InventoryData inventoryData = addLocation.inventoryData;

        if (inventoryData != null && inventoryData.CanAddItemAtLocation(itemData, addLocation))
        {
            inventoryData.AddItemAtLocation(itemData, addLocation);

            OnItemDataAdded?.Invoke(itemData, addLocation);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveItemData(ItemData itemData, InventoryData inventoryData)
    {
        if (inventoryData != null && inventoryData.CanRemoveItem(itemData))
        {
            ItemLocation removeLocation = new(inventoryData, itemData.gridID, itemData.gridIndex, itemData.IsItemRotated);
            inventoryData.RemoveItem(itemData);

            OnItemDataRemoved?.Invoke(removeLocation);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MoveItemData(ItemData itemData, ItemLocation fromLocation, ItemLocation toLocation)
    {
        InventoryData fromInventoryData = fromLocation.inventoryData;
        InventoryData toInventoryData = toLocation.inventoryData;

        if (fromInventoryData == null || toInventoryData == null || fromLocation == toLocation)
        {
            return false;
        }

        if (fromInventoryData.CanRemoveItem(itemData)
            && toInventoryData.CanAddItemAtLocation(itemData, toLocation))
        {
            fromInventoryData.RemoveItem(itemData);
            toInventoryData.AddItemAtLocation(itemData, toLocation);

            OnItemDataMoved?.Invoke(itemData, fromLocation, toLocation);

            return true;
        }
        else
        {
            return false;
        }
    }
}
