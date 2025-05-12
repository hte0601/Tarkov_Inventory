using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RowColumn
{
    public int row;
    public int col;
}

public class InventoryUI : UIBase
{
    [SerializeField] protected RowColumn inventorySize;

    protected RowColumn EventPosToInventoryIndex(Vector2 eventPos)
    {
        RowColumn index;

        ScreenPointToLocalPoint(eventPos, out Vector2 point, OriginPoint.TopLeft);

        // index 계산
        index.row = (int)(point.y / (rectTransform.rect.height / inventorySize.row));
        index.col = (int)(point.x / (rectTransform.rect.width / inventorySize.col));

        // 계산 오차에 의해 index 범위를 벗어나는 경우 방지
        if (index.row < 0)
        {
            index.row = 0;
        }
        else if (inventorySize.row <= index.row)
        {
            index.row = inventorySize.row - 1;
        }

        if (index.col < 0)
        {
            index.col = 0;
        }
        else if (inventorySize.col <= index.col)
        {
            index.col = inventorySize.col - 1;
        }

        return index;
    }
}
