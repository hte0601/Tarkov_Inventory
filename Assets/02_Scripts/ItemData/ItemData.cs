using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    [Serializable]
    public struct ItemSizeData
    {
        public int width;
        public int height;
    }

    [SerializeField] private ItemSizeData _itemSize;  // 임시 SerializeField

    public ItemSizeData ItemSize
    {
        get { return _itemSize; }
        private set { _itemSize = value; }
    }
}
