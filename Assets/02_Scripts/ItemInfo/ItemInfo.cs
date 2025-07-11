public class ItemInfo
{
    public ItemID ID { get; private set; }
    public string IconPath { get; private set; }
    public ItemCategoryData Category { get; private set; }
    public ItemSizeData ItemSize { get; private set; }

    public ItemInfo(ItemID itemID, string iconPath, ItemCategoryData category, ItemSizeData itemSize)
    {
        ID = itemID;
        IconPath = iconPath;
        Category = category;
        ItemSize = itemSize;
    }
}
