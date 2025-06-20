public interface IInventory
{
    public void HandleItemDragBegin(int gridID, ItemUI item);
    public void HandleItemDragCancle(int gridID, ItemUI item);
    public void HandleItemDragOver(int gridID, RowColumn mouseIndex, RowColumn dragIndex, ItemUI item);
    public void HandleItemDrop(int gridID, RowColumn dropIndex, ItemUI item);
}
