public class ItemInfo
{
    public ItemSizeData ItemSize { get; private set; }
    public ItemCategoryData Category { get; private set; }

    public ItemInfo(ItemSizeData itemSize, ItemCategoryData category)
    {
        ItemSize = itemSize;
        Category = category;
    }
}
