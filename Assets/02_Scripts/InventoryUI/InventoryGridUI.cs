using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridUI : UIBase, IDropHandler
{
    public const int SLOT_SIZE = 63;
    public const int PADDING = 1;

    public event Action<int, ItemUI> OnItemDoubleClick;
    public event Action<int, ItemUI> OnItemDragBegin;
    public event Action<int, ItemUI> OnItemDragEnd;
    public event Action<int, ItemDragContext, RowColumn, RowColumn> OnItemDrop;
    public event Action<int, ItemDragContext, RowColumn, RowColumn> OnItemDragOver;

    [SerializeField] private GameObject itemRootObj;
    [SerializeField] private DropIndicatorUI dropIndicator;
    [SerializeField] private RowColumn _gridSize;  // 인스펙터에서 값 설정

    private Transform itemRootTransform;
    private int gridID;

    public RowColumn GridSize
    {
        get { return _gridSize; }
    }


    public void Initialize(int gridID)
    {
        itemRootTransform = itemRootObj.transform;

        this.gridID = gridID;
    }


    public void AddItemUIAtIndex(ItemUI itemUI, RowColumn gridIndex, bool isItemUIRotated)
    {
        itemUI.OnDoubleClick += HandleItemUIDoubleClick;
        itemUI.OnDragBegin += HandleItemUIDragBegin;
        itemUI.OnDragEnd += HandleItemUIDragEnd;
        itemUI.OnDropped += HandleItemUIDropped;

        itemUI.transform.SetParent(itemRootTransform);

        Vector2 localPosition = GridIndexToLocalPoint(gridIndex) - itemUI.GetTopLeftCellOffset(isItemUIRotated);
        Quaternion localRotation = isItemUIRotated ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;
        itemUI.transform.SetLocalPositionAndRotation(localPosition, localRotation);
    }

    public void RemoveItemUI(ItemUI itemUI)
    {
        itemUI.OnDoubleClick -= HandleItemUIDoubleClick;
        itemUI.OnDragBegin -= HandleItemUIDragBegin;
        itemUI.OnDragEnd -= HandleItemUIDragEnd;
        itemUI.OnDropped -= HandleItemUIDropped;
    }

    public void SetIndicator(RowColumn gridIndex, RowColumn indicatorSize, bool canDrop)
    {
        dropIndicator.SetIndicator(gridIndex, indicatorSize, canDrop);
    }


    // ItemUI의 마우스 이벤트에 대한 핸들러
    private void HandleItemUIDoubleClick(ItemUI itemUI, PointerEventData eventData)
    {
        OnItemDoubleClick?.Invoke(gridID, itemUI);
    }

    private void HandleItemUIDragBegin(ItemUI itemUI, PointerEventData eventData)
    {
        OnItemDragBegin?.Invoke(gridID, itemUI);
    }

    private void HandleItemUIDragEnd(ItemUI itemUI, PointerEventData eventData)
    {
        OnItemDragEnd?.Invoke(gridID, itemUI);
    }

    private void HandleItemUIDropped(ItemUI itemUI, PointerEventData eventData)
    {
        OnDrop(eventData);
    }


    // IDropHandler 구현
    public void OnDrop(PointerEventData eventData)
    {
        ItemDragContext dragContext = ItemDragManager.instance.dragContext;

        if (eventData.button == PointerEventData.InputButton.Left && dragContext.IsDragging)
        {
            Vector2 mousePosition = eventData.position;
            Vector2 itemPosition = mousePosition + dragContext.ItemUITopLeftCellOffset;

            ScreenPointToGridIndex(mousePosition, out RowColumn mouseIndex);
            ScreenPointToGridIndex(itemPosition, out RowColumn itemIndex);

            OnItemDrop?.Invoke(gridID, dragContext, mouseIndex, itemIndex);
        }
    }


    // DragOver 관련 메소드 (ItemDragManager로부터 호출됨)
    public void OnDragOverBegin()
    {
        dropIndicator.BeginDragOver();
    }

    public void OnDragOverEnd()
    {
        dropIndicator.EndDragOver();
    }

    public void OnDragOver(ItemDragContext dragContext, Vector2 mousePosition)
    {
        Vector2 itemPosition = mousePosition + dragContext.ItemUITopLeftCellOffset;

        ScreenPointToGridIndex(mousePosition, out RowColumn mouseIndex);
        ScreenPointToGridIndex(itemPosition, out RowColumn itemIndex);

        OnItemDragOver?.Invoke(gridID, dragContext, mouseIndex, itemIndex);
    }


    // 좌표 변환 메소드들
    private RowColumn LocalPointToGridIndex(Vector2 localPoint)
    {
        RowColumn gridIndex;
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x += rectTransform.pivot.x * width;
        localPoint.y += rectTransform.pivot.y * height;
        localPoint.y = height - localPoint.y;

        gridIndex.row = Mathf.FloorToInt((localPoint.y - PADDING) / SLOT_SIZE);
        gridIndex.col = Mathf.FloorToInt((localPoint.x - PADDING) / SLOT_SIZE);

        // if (indexClamping)
        // {
        //     gridIndex.row = Mathf.Clamp(gridIndex.row, 0, GridSize.row - 1);
        //     gridIndex.col = Mathf.Clamp(gridIndex.col, 0, GridSize.col - 1);
        // }

        return gridIndex;
    }

    private bool ScreenPointToGridIndex(Vector2 screenPoint, out RowColumn gridIndex)
    {
        if (ScreenPointToLocalPoint(screenPoint, out Vector2 localPoint))
        {
            gridIndex = LocalPointToGridIndex(localPoint);

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

        localPoint.x = (gridIndex.col + 0.5f) * SLOT_SIZE + PADDING;
        localPoint.y = (gridIndex.row + 0.5f) * SLOT_SIZE + PADDING;
        localPoint.y *= -1f;

        return localPoint;
    }
}
