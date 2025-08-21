using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalRigItemData : GearItemData, IContainableItemData
{
    public InventoryData InnerInventoryData { get; private set; }

    public TacticalRigItemData(TacticalRigItemInfo itemInfo) : base(itemInfo)
    {
        InnerInventoryData = new(itemInfo.InventorySize);
    }
}
