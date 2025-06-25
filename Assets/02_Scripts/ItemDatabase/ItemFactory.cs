using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemData CreateItemData(ItemID itemID)
    {
        ItemData itemData = null;
        ItemInfo itemInfo = ItemDatabase.info[itemID];

        if (itemInfo.Category.Matches(ItemMainCategory1.BarterItems)
            || itemInfo.Category.Matches(ItemMainCategory1.InfoItems))
        {
            itemData = new(itemInfo);
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
