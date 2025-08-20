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

    private readonly int gridID;
    private readonly RowColumn gridSize;
    private readonly SlotData[,] slots;

    public InventoryGridData(int gridID, RowColumn gridSize)
    {
        this.gridID = gridID;
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


    public bool TryGetItemAtIndex(RowColumn gridIndex, out ItemData item)
    {
        if (!slots[gridIndex.row, gridIndex.col].isSlotEmpty)
        {
            item = slots[gridIndex.row, gridIndex.col].itemInSlot;
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }


    public bool CanAddItemAtIndex(ItemData item, RowColumn gridIndex, bool isItemRotated)
    {
        RowColumn itemSize = item.GetItemSize(isItemRotated);

        if (gridIndex.row < 0 || gridSize.row <= gridIndex.row + itemSize.row - 1)
        {
            return false;
        }

        if (gridIndex.col < 0 || gridSize.col <= gridIndex.col + itemSize.col - 1)
        {
            return false;
        }

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                ref SlotData slot = ref slots[gridIndex.row + r, gridIndex.col + c];

                if (!slot.isSlotEmpty && !ReferenceEquals(slot.itemInSlot, item))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddItemAtIndex(ItemData item, RowColumn gridIndex, bool isItemRotated)
    {
        item.gridID = gridID;
        item.gridIndex = gridIndex;
        item.IsItemRotated = isItemRotated;
        RowColumn itemSize = item.ItemSize;

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                slots[gridIndex.row + r, gridIndex.col + c].isSlotEmpty = false;
                slots[gridIndex.row + r, gridIndex.col + c].itemInSlot = item;
            }
        }
    }

    public bool CanRemoveItem(ItemData item)
    {
        ItemData itemInSlot = slots[item.gridIndex.row, item.gridIndex.col].itemInSlot;

        return ReferenceEquals(item, itemInSlot);
    }

    public void RemoveItem(ItemData item)
    {
        RowColumn gridIndex = item.gridIndex;
        RowColumn itemSize = item.ItemSize;

        for (int r = 0; r < itemSize.row; r++)
        {
            for (int c = 0; c < itemSize.col; c++)
            {
                slots[gridIndex.row + r, gridIndex.col + c].isSlotEmpty = true;
                slots[gridIndex.row + r, gridIndex.col + c].itemInSlot = null;
            }
        }

        item.gridID = 0;
        item.gridIndex = new RowColumn(0, 0);
        item.IsItemRotated = false;
    }


    public bool TryFindIndexToAddItem(ItemData item, out RowColumn gridIndex, out bool isItemRotated)
    {
        bool isItemSquare = item.ItemSize.row == item.ItemSize.col;

        for (int r = 0; r < gridSize.row; r++)
        {
            for (int c = 0; c < gridSize.col; c++)
            {
                gridIndex = new RowColumn(r, c);

                if (CanAddItemAtIndex(item, gridIndex, false))
                {
                    isItemRotated = false;
                    return true;
                }

                if (!isItemSquare && CanAddItemAtIndex(item, gridIndex, true))
                {
                    isItemRotated = true;
                    return true;
                }
            }
        }

        gridIndex = new RowColumn(0, 0);
        isItemRotated = false;
        return false;
    }
}
