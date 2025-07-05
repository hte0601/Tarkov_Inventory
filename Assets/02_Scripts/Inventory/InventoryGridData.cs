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


    // 아이템이 들어갈 수 있는 자리가 있는지
    public bool CanAddItemToGrid()
    {
        return false;
    }

    public bool AddItemToGrid()
    {
        return false;
    }
}
