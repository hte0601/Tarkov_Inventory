using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFactory
{
    public static ItemData CreateItemData(ItemID itemID)
    {
        if (!ItemDatabase.TryGetItemInfo(itemID, out ItemInfo itemInfo))
        {
            return null;
        }

        ItemData itemData = null;

        if (itemInfo.Category.Matches(ItemMainCategory1.BarterItems)
            || itemInfo.Category.Matches(ItemMainCategory1.InfoItems))
        {
            itemData = new ItemData(itemInfo);
        }
        else if (itemInfo.Category.Matches(ItemMainCategory1.Gear, GearSubCategory2.Backpacks))
        {
            itemData = new BackpackItemData(itemInfo as BackpackItemInfo);
        }
        else if (itemInfo.Category.Matches(ItemMainCategory1.Gear, GearSubCategory2.TacticalRigs))
        {
            itemData = new TacticalRigItemData(itemInfo as TacticalRigItemInfo);
        }

        return itemData;
    }
}
