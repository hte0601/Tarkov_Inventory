public interface IInventory
{
    public void HandleItemBeginDrag(int gridID, RowColumn itemIndex, ItemUI item);
    public void HandleItemCancleDrag(int gridID, RowColumn itemIndex, ItemUI item);
    public void HandleItemDragOver(int gridID, RowColumn eventIndex, ItemUI item);
    public void HandleItemDropOn(int gridID, RowColumn eventIndex, ItemUI item);
}
