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


    public void OnItemBeginDrag(ItemUI item, RowColumn index)
    {
        // 논리적으로는 불가능한 경우임
        if (ItemDragManager.instance.IsDragging)
            return;

        // RemoveItemFromInventory(index);
        ItemDragManager.instance.BeginItemDrag(item);
    }

    public void OnItemEndDrag(ItemUI item, RowColumn index)
    {
        if (!ItemDragManager.instance.IsDragging)
            return;
        // 레퍼런스 이퀄 체크?
        // 추가 처리 필요?

        ItemDragManager.instance.CancleItemDrag(this);
        AddItemToInventory(item, index);
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!ItemDragManager.instance.IsDragging)
            return;

        // 드랍 가능한지 조건 체크

        ItemUI item = ItemDragManager.instance.DropItem(this);
        Vector2 screenPoint = eventData.position + item.originPoint;

        ScreenPointToInventoryIndex(screenPoint, out RowColumn index, false);
        AddItemToInventory(item, index);

        Debug.LogFormat("({0}, {1}) 드래그 됨", index.row, index.col);
    }


    private void AddItemToInventory(ItemUI item, RowColumn index)
    {
        Vector2 localPoint = InventoryIndexToLocalPoint(index);

        item.transform.localPosition = localPoint - item.originPoint;
        item.MoveItemTo(this, index);
    }

    private void RemoveItemFromInventory(RowColumn index)
    {
        // 구현 필요
    }
}
