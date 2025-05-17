using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    [Serializable]
    public struct ItemSize
    {
        public int width;
        public int height;
    }

    [SerializeField] private ItemSize _gridSize;  // 임시

    public ItemSize GridSize
    {
        get { return _gridSize; }
        private set { _gridSize = value; }
    }
}
