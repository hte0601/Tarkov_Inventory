using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGridData
{
    private struct SlotData
    {
        public bool isSlotEmpty;
        public ItemData itemInSlot;
    }

    private RowColumn gridSize;
    private SlotData[,] slots;

    public InventoryGridData(RowColumn gridSize)
    {
        this.gridSize = gridSize;
        slots = new SlotData[gridSize.row, gridSize.col];

        for (int r = 0; r < gridSize.row; r++)
        {
            for (int c = 0; c < gridSize.col; c++)
            {
                slots[r, c].isSlotEmpty = true;
                slots[r, c].itemInSlot = null;
            }
        }
    }


    public bool TryGetItemAtIndex(RowColumn index, out ItemData item)
    {
        if (!slots[index.row, index.col].isSlotEmpty)
        {
            item = slots[index.row, index.col].itemInSlot;
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }


    public bool CanAddItemAtIndex(RowColumn index, bool isRotated, ItemData item)
    {
        RowColumn itemSize = item.GetItemSize(isRotated);

        if (index.row < 0 || gridSize.row <= index.row + itemSize.row - 1)
        {
            return false;
        }

        if (index.col < 0 || gridSize.col <= index.col + itemSize.col - 1)
        {
            return false;
        }

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                ref SlotData slot = ref slots[index.row + r, index.col + c];

                if (!slot.isSlotEmpty && !ReferenceEquals(slot.itemInSlot, item))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool AddItemAtIndex(RowColumn index, bool isRotated, ItemData item)
    {
        if (!CanAddItemAtIndex(index, isRotated, item))
        {
            return false;
        }

        item.GridIndex = index;
        item.IsRotated = isRotated;
        RowColumn itemSize = item.ItemSize;

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                slots[index.row + r, index.col + c].isSlotEmpty = false;
                slots[index.row + r, index.col + c].itemInSlot = item;
            }
        }

        return true;
    }

    public void RemoveItem(ItemData item)
    {
        RowColumn index = item.GridIndex;
        RowColumn itemSize = item.ItemSize;

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                slots[index.row + r, index.col + c].isSlotEmpty = true;
                slots[index.row + r, index.col + c].itemInSlot = null;
            }
        }
    }


    public bool TryFindIndexToAddItem(ItemData item, out RowColumn index, out bool isRotated)
    {
        bool isItemSquare = item.ItemSize.row == item.ItemSize.col;

        for (int r = 0; r < gridSize.row; r++)
        {
            for (int c = 0; c < gridSize.col; c++)
            {
                index = new(r, c);

                if (CanAddItemAtIndex(index, false, item))
                {
                    isRotated = false;
                    return true;
                }

                if (!isItemSquare && CanAddItemAtIndex(index, true, item))
                {
                    isRotated = true;
                    return true;
                }
            }
        }

        index = new(0, 0);
        isRotated = false;
        return false;
    }
}
