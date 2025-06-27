using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryUIManager
{
    private static readonly Dictionary<InventoryData, InventoryUI> inventoryUIDict;

    static InventoryUIManager()
    {
        inventoryUIDict = new();
    }


    public static bool OpenUI(InventoryData inventoryData, InventoryUI inventoryUI)
    {
        return inventoryUIDict.TryAdd(inventoryData, inventoryUI);
    }

    public static bool CloseUI(InventoryData inventoryData)
    {
        return inventoryUIDict.Remove(inventoryData);
    }

    public static bool TryGetUI(InventoryData inventoryData, out InventoryUI inventoryUI)
    {
        return inventoryUIDict.TryGetValue(inventoryData, out inventoryUI);
    }
}
