using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridUI : UIBase, IDropHandler, IPointerDownHandler
{
    [SerializeField] private GameObject itemListObj;
    [SerializeField] private DropIndicatorUI dropIndicator;

    public const int SLOT_SIZE = 63;
    public const int PADDING = 1;

    private IInventoryUI parentInventoryUI;
    private int gridID;
    [SerializeField] private RowColumn _gridSize;  // 인스펙터에서 값 설정

    public Transform ItemListTransform { get; private set; }

    public RowColumn GridSize
    {
        get { return _gridSize; }
    }


    public void Initialize(IInventoryUI parentInventoryUI, int gridID)
    {
        ItemListTransform = itemListObj.transform;

        this.parentInventoryUI = parentInventoryUI;
        this.gridID = gridID;
    }


    public void PlaceItemUIAtIndex(RowColumn index, bool isRotated, ItemUI itemUI)
    {
        itemUI.parentGridUI = this;
        itemUI.IsUIRotated = isRotated;

        itemUI.transform.SetParent(ItemListTransform);
        itemUI.transform.localPosition = GridIndexToLocalPoint(index) - itemUI.TopLeftSlotPoint;
    }

    public void SetIndicator(RowColumn index, RowColumn size, bool canDrop)
    {
        dropIndicator.SetIndicator(index, size, canDrop);
    }


    // 아이템 Drag 관련 메소드 (ItemUI로부터 호출됨)
    public void OnItemBeginDrag(ItemUI item)
    {
        if (!ItemDragManager.instance.IsDragging)
        {
            parentInventoryUI.HandleItemDragBegin(gridID, item);
        }
    }


    // IDropHandler 구현 (ItemUI로부터 호출될 수 있음)
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && ItemDragManager.instance.IsDragging)
        {
            Vector2 mousePosition = eventData.position;
            ItemUI draggingItem = ItemDragManager.instance.DraggingItem;

            ScreenPointToGridIndex(mousePosition, out RowColumn mouseIndex, false);
            RowColumn itemIndex;

            if (draggingItem.UISize.row == 1 && draggingItem.UISize.col == 1)
            {
                itemIndex = mouseIndex;
            }
            else
            {
                Vector2 itemPosition = mousePosition + draggingItem.TopLeftSlotPoint;
                ScreenPointToGridIndex(itemPosition, out itemIndex, false);
            }

            parentInventoryUI.HandleItemDrop(gridID, mouseIndex, itemIndex, draggingItem);
        }
    }


    // DragOver 관련 메소드 (ItemDragManager로부터 호출됨)
    public void OnBeginDragOver(ItemUI draggingItem)
    {
        dropIndicator.BeginDragOver(draggingItem.UISize);
    }

    public void OnEndDragOver()
    {
        dropIndicator.EndDragOver();
    }

    public void OnDragOver(Vector2 mousePosition, ItemUI draggingItem)
    {
        ScreenPointToGridIndex(mousePosition, out RowColumn mouseIndex, false);
        RowColumn itemIndex;

        if (draggingItem.UISize.row == 1 && draggingItem.UISize.col == 1)
        {
            itemIndex = mouseIndex;
        }
        else
        {
            Vector2 itemPosition = mousePosition + draggingItem.TopLeftSlotPoint;
            ScreenPointToGridIndex(itemPosition, out itemIndex, false);
        }

        parentInventoryUI.HandleItemDragOver(gridID, mouseIndex, itemIndex, draggingItem);
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

        gridIndex.row = Mathf.FloorToInt((localPoint.y - PADDING) / SLOT_SIZE);
        gridIndex.col = Mathf.FloorToInt((localPoint.x - PADDING) / SLOT_SIZE);

        if (indexClamping)
        {
            gridIndex.row = Mathf.Clamp(gridIndex.row, 0, GridSize.row - 1);
            gridIndex.col = Mathf.Clamp(gridIndex.col, 0, GridSize.col - 1);
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

        localPoint.x = (gridIndex.col + 0.5f) * SLOT_SIZE + PADDING;
        localPoint.y = (gridIndex.row + 0.5f) * SLOT_SIZE + PADDING;
        localPoint.y *= -1f;

        return localPoint;
    }


    // 테스트용
    public void OnPointerDown(PointerEventData eventData)
    {
        // ScreenPointToLocalPoint(eventData.position, out Vector2 localPoint);
        // Debug.Log(localPoint);
    }
}
