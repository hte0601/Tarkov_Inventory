using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGridData
{
    private struct SlotData
    {
        public bool isSlotEmpty;
        public ItemUI itemInSlot;
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


    // 범위 체크 함수
    private bool IsSlotsEmpty(RowColumn gridIndex, RowColumn itemSize)
    {
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
                if (!slots[gridIndex.row + r, gridIndex.col + c].isSlotEmpty)
                {
                    return false;
                }
            }
        }

        return true;
    }


    public bool CanAddItemAtIndex(RowColumn gridIndex, ItemUI item)
    {
        RowColumn itemSize = item.Size;

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

    public bool AddItemAtIndex(RowColumn gridIndex, ItemUI item)
    {
        if (!CanAddItemAtIndex(gridIndex, item))
        {
            return false;
        }

        for (int r = 0; r < item.Size.row; r++)
        {
            for (int c = 0; c < item.Size.col; c++)
            {
                slots[gridIndex.row + r, gridIndex.col + c].isSlotEmpty = false;
                slots[gridIndex.row + r, gridIndex.col + c].itemInSlot = item;
            }
        }

        return true;
    }

    public void RemoveItem(ItemUI item)
    {
        RowColumn gridIndex = item.Index;

        for (int r = 0; r < item.Size.row; r++)
        {
            for (int c = 0; c < item.Size.col; c++)
            {
                slots[gridIndex.row + r, gridIndex.col + c].isSlotEmpty = true;
                slots[gridIndex.row + r, gridIndex.col + c].itemInSlot = null;
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
