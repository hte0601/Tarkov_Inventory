using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    private struct SlotData
    {
        public bool isSlotEmpty;
        public ItemUI itemInSlot;
    }

    private RowColumn inventorySize;
    private SlotData[,] slots;

    public InventoryData(RowColumn inventorySize)
    {
        this.inventorySize = inventorySize;
        slots = new SlotData[inventorySize.row, inventorySize.col];

        for (int r = 0; r < inventorySize.row; r++)
        {
            for (int c = 0; c < inventorySize.col; c++)
            {
                slots[r, c].isSlotEmpty = true;
                slots[r, c].itemInSlot = null;
            }
        }
    }


    // 범위 체크 함수
    private bool IsSlotsEmpty(RowColumn index, RowColumn gridSize)
    {
        if (index.row < 0 || inventorySize.row <= index.row + gridSize.row - 1)
        {
            return false;
        }

        if (index.col < 0 || inventorySize.col <= index.col + gridSize.col - 1)
        {
            return false;
        }

        for (int r = 0; r < gridSize.row; r++)
        {
            for (int c = 0; c < gridSize.col; c++)
            {
                if (!slots[index.row + r, index.col + c].isSlotEmpty)
                {
                    return false;
                }
            }
        }

        return true;
    }


    public bool CanAddItemAtIndex(ItemUI item, RowColumn index)
    {
        return IsSlotsEmpty(index, item.GridSize);
    }

    public bool AddItemAtIndex(ItemUI item, RowColumn index)
    {
        if (!CanAddItemAtIndex(item, index))
        {
            return false;
        }

        for (int r = 0; r < item.GridSize.row; r++)
        {
            for (int c = 0; c < item.GridSize.col; c++)
            {
                slots[index.row + r, index.col + c].isSlotEmpty = false;
                slots[index.row + r, index.col + c].itemInSlot = item;
            }
        }

        return true;
    }

    public void RemoveItem(ItemUI item)
    {
        RowColumn index = item.Index;

        for (int r = 0; r < item.GridSize.row; r++)
        {
            for (int c = 0; c < item.GridSize.col; c++)
            {
                slots[index.row + r, index.col + c].isSlotEmpty = true;
                slots[index.row + r, index.col + c].itemInSlot = null;
            }
        }
    }


    // 아이템이 들어갈 수 있는 자리가 있는지
    public bool CanAddItemToInventory()
    {
        return false;
    }

    public bool AddItemToInventory()
    {
        return false;
    }
}
