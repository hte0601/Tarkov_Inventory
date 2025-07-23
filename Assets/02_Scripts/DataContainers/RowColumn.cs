using System;

[Serializable]
public struct RowColumn : IEquatable<RowColumn>, IComparable<RowColumn>
{
    public int row;
    public int col;

    public RowColumn(int row, int col)
    {
        this.row = row;
        this.col = col;
    }


    public readonly bool Equals(RowColumn other)
    {
        return row == other.row && col == other.col;
    }

    public readonly int CompareTo(RowColumn other)
    {
        if (row < other.row) return -1;
        else if (row > other.row) return 1;

        if (col < other.col) return -1;
        else if (col > other.col) return 1;

        return 0;
    }

    public static bool operator ==(RowColumn lhs, RowColumn rhs) => lhs.Equals(rhs);
    public static bool operator !=(RowColumn lhs, RowColumn rhs) => !lhs.Equals(rhs);

    public override readonly bool Equals(object obj) => obj is RowColumn other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(row, col);
}
