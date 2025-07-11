using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public ItemID ID { get; private set; }  // 저장 o
    public string IconPath { get; private set; }  // static
    public ItemCategoryData Category { get; private set; }  // static

    private ItemSizeData itemBaseSize;  // static
    private ItemSizeData _itemCurrentSize;  // 저장 ?
    public RowColumn ItemSize { get; protected set; }  // 저장 x
    public RowColumn GridIndex { get; set; }  // 저장 x
    private bool _isRotated;  // 저장 o

    public ItemSizeData ItemCurrentSize
    {
        get { return _itemCurrentSize; }
        protected set
        {
            _itemCurrentSize = value;
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
        itemBaseSize = itemInfo.ItemSize;

        ItemCurrentSize = itemBaseSize;
        GridIndex = new(0, 0);
        IsRotated = false;
    }


    public RowColumn GetItemSize(bool isRotated)
    {
        RowColumn itemSize;
        itemSize.row = isRotated ? ItemCurrentSize.width : ItemCurrentSize.height;
        itemSize.col = isRotated ? ItemCurrentSize.height : ItemCurrentSize.width;

        return itemSize;
    }
}
