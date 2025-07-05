using System;

public struct ItemLocation : IEquatable<ItemLocation>
{
    public InventoryData inventoryData;
    public int gridID;
    public RowColumn index;
    public bool isRotated;

    public ItemLocation(InventoryData inventoryData, int gridID, RowColumn index, bool isRotated)
    {
        this.inventoryData = inventoryData;
        this.gridID = gridID;
        this.index = index;
        this.isRotated = isRotated;
    }


    public readonly bool Equals(ItemLocation other)
    {
        return ReferenceEquals(inventoryData, other.inventoryData)
            && gridID == other.gridID && index == other.index && isRotated == other.isRotated;
    }

    public static bool operator ==(ItemLocation lhs, ItemLocation rhs) => lhs.Equals(rhs);
    public static bool operator !=(ItemLocation lhs, ItemLocation rhs) => !lhs.Equals(rhs);

    public override readonly bool Equals(object obj) => obj is ItemLocation other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(inventoryData, gridID, index, isRotated);
}
