using System;

public interface IInventoryUI
{
    public event Action<IInventoryUI, ItemUI> OnItemDragBegin;
    public event Action<IInventoryUI, ItemUI> OnItemDragEnd;
    public event Action<IInventoryUI, ItemUI, ItemLocation> OnItemDropOnInventoryGrid;
    public event Action<IInventoryUI, ItemUI, IContainableItemData> OnItemDropOnContainableItem;

    public InventoryData Data { get; }

    public void SetupUI(InventoryData inventoryData);
    public void ResetUI();
    public bool AddItemUI(ItemUI itemUI);
    public bool RemoveItemUI(int gridID, RowColumn gridIndex, out ItemUI itemUI);

    // IInventoryGridUIEventHandler로 분리
    public void HandleItemDragBegin(ItemUI itemUI, int gridID);
    public void HandleItemDragEnd(ItemUI itemUI, int gridID);
    public void HandleItemDrop(ItemDragContext dragContext, int gridID, RowColumn mouseIndex, RowColumn dropIndex);
    public void HandleItemDragOver(ItemDragContext dragContext, int gridID, RowColumn mouseIndex, RowColumn dragOverIndex);
}
