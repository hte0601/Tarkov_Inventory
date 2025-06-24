using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public ItemSizeData ItemSize { get; private set; }
    public ItemCategoryData Category { get; private set; }

    public ItemData(ItemInfo itemInfo)
    {
        ItemSize = itemInfo.ItemSize;
        Category = itemInfo.Category;
    }
}
