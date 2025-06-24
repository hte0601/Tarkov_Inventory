public class BackpackItemInfo : GearItemInfo
{
    public RowColumn[] InventorySize { get; private set; }

    public BackpackItemInfo(ItemSizeData itemSize, ItemCategoryData category, RowColumn[] inventorySize)
        : base(itemSize, category)
    {
        InventorySize = inventorySize;
    }
}
