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

    public int gridID;  // 저장 o
    public RowColumn gridIndex;  // 저장 o
    private bool _isItemRotated;  // 저장 o

    public ItemSizeData ItemSizeData
    {
        get { return _itemSizeData; }
        protected set
        {
            _itemSizeData = value;
            ItemSize = GetItemSize(IsItemRotated);
        }
    }

    public bool IsItemRotated
    {
        get { return _isItemRotated; }
        set
        {
            _isItemRotated = value;
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

        gridID = 0;
        gridIndex = new RowColumn(0, 0);
        IsItemRotated = false;
    }


    public RowColumn GetItemSize(bool isRotated)
    {
        RowColumn itemSize;
        itemSize.row = isRotated ? ItemSizeData.width : ItemSizeData.height;
        itemSize.col = isRotated ? ItemSizeData.height : ItemSizeData.width;

        return itemSize;
    }
}
