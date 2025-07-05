using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySaveData
{
    public List<SortedDictionary<RowColumn, ItemData>> itemList;

    public InventorySaveData()
    {
        itemList = new();
    }
}
