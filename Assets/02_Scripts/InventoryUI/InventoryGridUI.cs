using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public struct RowColumn
{
    public int row;
    public int col;
}


public class InventoryGridUI : UIBase, IDropHandler
{
    public const int SLOT_SIZE = 64;

    [SerializeField] private RowColumn gridSize;
    private InventoryData data;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        data = new(gridSize);
    }


    public void OnItemBeginDrag(ItemUI item)
    {
        // 논리적으로는 불가능한 경우임
        if (ItemDragManager.instance.IsDragging)
        {
            return;
        }

        RemoveItemFromInventory(item);
        ItemDragManager.instance.BeginItemDrag(item);
    }

    public void OnItemEndDrag(ItemUI item)
    {
        if (!ItemDragManager.instance.IsDragging)
        {
            return;
        }

        // 레퍼런스 이퀄 체크?

        ItemDragManager.instance.CancleItemDrag(this);
        AddItemToInventory(item, item.Index);
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!ItemDragManager.instance.IsDragging)
        {
            return;
        }

        // drop 이벤트 좌표로부터 index 계산
        ItemUI item = ItemDragManager.instance.GetDraggingItem();
        Vector2 screenPoint = eventData.position + item.TopLeftSlotPoint;
        ScreenPointToGridIndex(screenPoint, out RowColumn index, false);

        if (data.CanAddItemAtIndex(item, index))
        {
            ItemDragManager.instance.DropItemToInventoryGrid(this);
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
        item.transform.localPosition = GridIndexToLocalPoint(index) - item.TopLeftSlotPoint;
        item.UpdateLocation(this, index);
        data.AddItemAtIndex(item, index);
    }

    private void RemoveItemFromInventory(ItemUI item)
    {
        data.RemoveItem(item);
    }


    // 좌표 변환 메소드들
    private RowColumn LocalPointToGridIndex(Vector2 localPoint, bool indexClamping)
    {
        RowColumn gridIndex;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x += rectTransform.pivot.x * width;
        localPoint.y += rectTransform.pivot.y * height;
        localPoint.y = height - localPoint.y;

        gridIndex.row = Mathf.FloorToInt(localPoint.y / SLOT_SIZE);
        gridIndex.col = Mathf.FloorToInt(localPoint.x / SLOT_SIZE);

        if (indexClamping)
        {
            gridIndex.row = Mathf.Clamp(gridIndex.row, 0, gridSize.row - 1);
            gridIndex.col = Mathf.Clamp(gridIndex.col, 0, gridSize.col - 1);
        }

        return gridIndex;
    }

    private bool ScreenPointToGridIndex(Vector2 screenPoint, out RowColumn gridIndex, bool indexClamping)
    {
        if (ScreenPointToLocalPoint(screenPoint, out Vector2 localPoint))
        {
            gridIndex = LocalPointToGridIndex(localPoint, indexClamping);

            return true;
        }
        else
        {
            gridIndex.row = 0;
            gridIndex.col = 0;

            return false;
        }
    }

    private Vector2 GridIndexToLocalPoint(RowColumn gridIndex)
    {
        Vector2 localPoint;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x = (gridIndex.col + 0.5f) * SLOT_SIZE;
        localPoint.y = (gridIndex.row + 0.5f) * SLOT_SIZE;
        localPoint.y = height - localPoint.y;

        localPoint.x -= rectTransform.pivot.x * width;
        localPoint.y -= rectTransform.pivot.y * height;

        return localPoint;
    }
}
