using System;

public interface IInventoryUI
{
    public event Action<IInventoryUI, ItemUI> OnItemDoubleClick;
    public event Action<IInventoryUI, ItemUI> OnItemDragBegin;
    public event Action<IInventoryUI, ItemUI> OnItemDragEnd;
    public event Action<IInventoryUI, ItemUI, ItemLocation> OnItemDropOnInventoryGrid;
    public event Action<IInventoryUI, ItemUI, IContainableItemData> OnItemDropOnContainableItem;

    public InventoryData Data { get; }

    public void SetupUI(InventoryData inventoryData);
    public void ResetUI();
    public bool AddItemUI(ItemUI itemUI);
    public bool RemoveItemUI(int gridID, RowColumn gridIndex, out ItemUI itemUI);
}
