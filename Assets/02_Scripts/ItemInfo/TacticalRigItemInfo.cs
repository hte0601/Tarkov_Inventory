public class TacticalRigItemInfo : GearItemInfo
{
    public RowColumn[] InventorySize { get; private set; }

    public TacticalRigItemInfo(ItemSizeData itemSize, ItemCategoryData category, RowColumn[] inventorySize)
        : base(itemSize, category)
    {
        InventorySize = inventorySize;
    }
}
