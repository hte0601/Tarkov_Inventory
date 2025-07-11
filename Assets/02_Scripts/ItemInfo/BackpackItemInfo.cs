public class BackpackItemInfo : GearItemInfo
{
    public RowColumn[] InventorySize { get; private set; }

    public BackpackItemInfo(ItemID itemID, string iconPath, ItemCategoryData category, ItemSizeData itemSize, RowColumn[] inventorySize)
        : base(itemID, iconPath, category, itemSize)
    {
        InventorySize = inventorySize;
    }
}
