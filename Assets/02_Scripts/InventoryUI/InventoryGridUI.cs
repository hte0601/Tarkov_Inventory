using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// IPointerDownHandler는 테스트용
public class InventoryGridUI : UIBase, IDropHandler, IPointerDownHandler
{
    [SerializeField] private GameObject itemListObj;
    [SerializeField] private DropIndicatorUI dropIndicator;

    public const int SLOT_SIZE = 63;
    public const int PADDING = 1;

    private IInventory inventory;
    private int gridID;
    [SerializeField] private RowColumn _gridSize;  // 인스펙터에서 값 설정

    public Transform ItemListTransform { get; private set; }

    public RowColumn GridSize
    {
        get { return _gridSize; }
    }


    public void Initialize(IInventory inventory, int gridID)
    {
        this.inventory = inventory;
        this.gridID = gridID;

        ItemListTransform = itemListObj.transform;
    }


    public void AddItemAtIndex(RowColumn gridIndex, ItemUI item)
    {
        item.transform.SetParent(ItemListTransform);
        item.transform.localPosition = GridIndexToLocalPoint(gridIndex) - item.TopLeftSlotPoint;
        item.UpdateLocation(this, gridIndex);
    }


    public void OnItemBeginDrag(ItemUI item)
    {
        if (!ItemDragManager.instance.IsDragging)
        {
            inventory.HandleItemBeginDrag(gridID, item.Index, item);
        }
    }

    public void OnItemEndDrag(ItemUI item)
    {
        if (ItemDragManager.instance.IsDragging
            && ReferenceEquals(item, ItemDragManager.instance.DraggingItem))
        {
            inventory.HandleItemCancleDrag(gridID, item.Index, item);
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (ItemDragManager.instance.IsDragging)
        {
            // drop 이벤트 좌표로부터 index 계산
            ItemUI item = ItemDragManager.instance.DraggingItem;
            Vector2 screenPoint = eventData.position + item.TopLeftSlotPoint;
            ScreenPointToGridIndex(screenPoint, out RowColumn eventIndex, false);

            inventory.HandleItemDropOn(gridID, eventIndex, item);
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        // ScreenPointToLocalPoint(eventData.position, out Vector2 localPoint);
        // Debug.Log(localPoint);
    }
}
