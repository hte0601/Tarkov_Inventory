using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItemData : GearItemData, IContainableItem
{
    public InventoryData InnerInventoryData { get; private set; }

    public BackpackItemData(BackpackItemInfo backpackItemInfo) : base(backpackItemInfo)
    {
        InnerInventoryData = new(backpackItemInfo.InventorySize);
    }
}
