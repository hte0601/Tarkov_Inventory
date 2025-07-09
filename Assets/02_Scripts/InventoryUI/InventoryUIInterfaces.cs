public interface IInventoryUI
{
    public void PlaceItemUIAtLocation(ItemLocation location, ItemUI item);
    public void HandleItemDragBegin(int gridID, ItemUI item);
    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI itemUI);
    public void HandleItemDrop(int gridID, RowColumn mouseIndex, RowColumn dropIndex, ItemUI itemUI);
}
