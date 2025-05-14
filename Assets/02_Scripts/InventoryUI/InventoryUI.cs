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
    public const int SLOT_SIZE = 64;

    [SerializeField] protected RowColumn inventorySize;

    protected RowColumn LocalPointToInventoryIndex(Vector2 localPoint, bool clampIndex)
    {
        RowColumn index;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x += rectTransform.pivot.x * width;
        localPoint.y += rectTransform.pivot.y * height;
        localPoint.y = height - localPoint.y;

        index.row = Mathf.FloorToInt(localPoint.y / SLOT_SIZE);
        index.col = Mathf.FloorToInt(localPoint.x / SLOT_SIZE);

        if (clampIndex)
        {
            index.row = Mathf.Clamp(index.row, 0, inventorySize.row - 1);
            index.col = Mathf.Clamp(index.col, 0, inventorySize.col - 1);
        }

        return index;
    }

    protected bool ScreenPointToInventoryIndex(Vector2 screenPoint, out RowColumn index, bool clampIndex)
    {
        if (ScreenPointToLocalPoint(screenPoint, out Vector2 localPoint))
        {
            index = LocalPointToInventoryIndex(localPoint, clampIndex);

            return true;
        }
        else
        {
            index.row = 0;
            index.col = 0;

            return false;
        }
    }

    protected Vector2 InventoryIndexToLocalPoint(RowColumn index)
    {
        Vector2 localPoint;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x = (index.col + 0.5f) * SLOT_SIZE;
        localPoint.y = (index.row + 0.5f) * SLOT_SIZE;
        localPoint.y = height - localPoint.y;

        localPoint.x -= rectTransform.pivot.x * width;
        localPoint.y -= rectTransform.pivot.y * height;

        return localPoint;
    }
}
