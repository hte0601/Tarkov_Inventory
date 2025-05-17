using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StashInventoryUI : InventoryUI, IDropHandler
{
    protected override void Awake()
    {
        base.Awake();
    }


    public void OnItemBeginDrag(ItemUI item)
    {
        // 논리적으로는 불가능한 경우임
        if (ItemDragManager.instance.IsDragging)
            return;

        RemoveItemFromInventory(item);
        ItemDragManager.instance.BeginItemDrag(item);
    }

    public void OnItemEndDrag(ItemUI item)
    {
        if (!ItemDragManager.instance.IsDragging)
            return;

        // 레퍼런스 이퀄 체크?

        ItemDragManager.instance.CancleItemDrag(this);
        AddItemToInventory(item, item.Index);
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!ItemDragManager.instance.IsDragging)
            return;

        // drop 이벤트 좌표로부터 index 계산
        ItemUI item = ItemDragManager.instance.GetDraggingItem();
        Vector2 screenPoint = eventData.position + item.TopLeftSlotPoint;
        ScreenPointToInventoryIndex(screenPoint, out RowColumn index, false);

        if (data.CanAddItemAtIndex(item, index))
        {
            ItemDragManager.instance.DropItemToInventory(this);
            AddItemToInventory(item, index);

            Debug.LogFormat("({0}, {1}) 드래그 성공", index.row, index.col);
        }
        else
        {
            Debug.LogFormat("({0}, {1}) 드래그 실패", index.row, index.col);
        }
    }


    private void AddItemToInventory(ItemUI item, RowColumn index)
    {
        item.transform.localPosition = InventoryIndexToLocalPoint(index) - item.TopLeftSlotPoint;
        item.UpdateLocation(this, index);
        data.AddItemAtIndex(item, index);
    }

    private void RemoveItemFromInventory(ItemUI item)
    {
        data.RemoveItem(item);
    }
}
