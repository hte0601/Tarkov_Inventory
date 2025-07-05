using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    protected ItemSizeData itemSizeData;
    [SerializeField] private bool _isRotated;  // 값 저장 필요

    public ItemCategoryData Category { get; protected set; }
    public RowColumn ItemSize { get; protected set; }
    public RowColumn GridIndex { get; set; }

    public bool IsRotated
    {
        get { return _isRotated; }
        set
        {
            _isRotated = value;

            RowColumn itemSize;
            itemSize.row = value ? itemSizeData.width : itemSizeData.height;
            itemSize.col = value ? itemSizeData.height : itemSizeData.width;

            ItemSize = itemSize;
        }
    }

    public ItemData(ItemInfo itemInfo)
    {
        itemSizeData = itemInfo.ItemSize;
        Category = itemInfo.Category;
        GridIndex = new(0, 0);
        IsRotated = false;
    }


    public RowColumn GetItemSize(bool isRotated)
    {
        RowColumn itemSize;
        itemSize.row = isRotated ? itemSizeData.width : itemSizeData.height;
        itemSize.col = isRotated ? itemSizeData.height : itemSizeData.width;

        return itemSize;
    }
}
