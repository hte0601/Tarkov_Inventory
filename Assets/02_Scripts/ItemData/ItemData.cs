using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public ItemID ID { get; private set; }  // 저장 o
    public string IconPath { get; private set; }  // static
    public ItemCategoryData Category { get; private set; }  // static

    private ItemSizeData itemBaseSizeData;  // static
    private ItemSizeData _itemSizeData;  // 저장 ?
    public RowColumn ItemSize { get; protected set; }  // 저장 x
    public RowColumn GridIndex { get; set; }  // 저장 x
    private bool _isRotated;  // 저장 o

    public ItemSizeData ItemSizeData
    {
        get { return _itemSizeData; }
        protected set
        {
            _itemSizeData = value;
            ItemSize = GetItemSize(IsRotated);
        }
    }

    public bool IsRotated
    {
        get { return _isRotated; }
        set
        {
            _isRotated = value;
            ItemSize = GetItemSize(value);
        }
    }


    public ItemData(ItemInfo itemInfo)
    {
        ID = itemInfo.ID;
        IconPath = itemInfo.IconPath;
        Category = itemInfo.Category;
        itemBaseSizeData = itemInfo.ItemSize;

        ItemSizeData = itemBaseSizeData;
        GridIndex = new(0, 0);
        IsRotated = false;
    }


    public RowColumn GetItemSize(bool isRotated)
    {
        RowColumn itemSize;
        itemSize.row = isRotated ? ItemSizeData.width : ItemSizeData.height;
        itemSize.col = isRotated ? ItemSizeData.height : ItemSizeData.width;

        return itemSize;
    }
}
