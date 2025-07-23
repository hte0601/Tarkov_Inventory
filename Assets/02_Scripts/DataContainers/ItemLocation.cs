using System;

public struct ItemLocation : IEquatable<ItemLocation>
{
    public InventoryData inventoryData;
    public int gridID;
    public RowColumn gridIndex;
    public bool isItemRotated;

    public ItemLocation(InventoryData inventoryData, int gridID, RowColumn gridIndex, bool isItemRotated)
    {
        this.inventoryData = inventoryData;
        this.gridID = gridID;
        this.gridIndex = gridIndex;
        this.isItemRotated = isItemRotated;
    }


    public readonly bool Equals(ItemLocation other)
    {
        return ReferenceEquals(inventoryData, other.inventoryData)
            && gridID == other.gridID && gridIndex == other.gridIndex && isItemRotated == other.isItemRotated;
    }

    public static bool operator ==(ItemLocation lhs, ItemLocation rhs) => lhs.Equals(rhs);
    public static bool operator !=(ItemLocation lhs, ItemLocation rhs) => !lhs.Equals(rhs);

    public override readonly bool Equals(object obj) => obj is ItemLocation other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(inventoryData, gridID, gridIndex, isItemRotated);
}
