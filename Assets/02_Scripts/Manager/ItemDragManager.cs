using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragManager : MonoBehaviour
{
    public static ItemDragManager instance;

    private RectTransform canvasRectTransform;
    private Camera canvasWorldCamera;

    private InventoryRaycaster inventoryRaycaster;
    private InventoryGridUI prevGridUI;

    private ItemLocation fromLocation;
    public ItemUI DraggingItem { get; private set; }
    public bool IsDragging { get; private set; }

    private void Awake()
    {
        instance = this;

        inventoryRaycaster = new();
        prevGridUI = null;

        DraggingItem = null;
        IsDragging = false;

        Canvas renderingCanvas = GetComponentInParent<Canvas>();
        if (renderingCanvas != null)
        {
            canvasWorldCamera = renderingCanvas.worldCamera;
            canvasRectTransform = renderingCanvas.transform as RectTransform;
        }
        else
        {
            Debug.LogError("InParent에서 Canvas 컴포넌트를 찾을 수 없음");
        }
    }

    private void Update()
    {
        if (!IsDragging)
        {
            return;
        }

        // 오브젝트를 마우스 위치로 이동
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, Input.mousePosition, canvasWorldCamera, out Vector2 localPoint))
        {
            transform.localPosition = localPoint;
        }

        // 아이템 회전 입력 처리
        if (Input.GetKeyDown(KeyCode.R))
        {
            DraggingItem.RotateItemUI();
        }

        // 아이템 DragOver 처리
        Vector2 mousePosition = Input.mousePosition;
        inventoryRaycaster.RaycastGridUI(mousePosition, out InventoryGridUI gridUI);

        if (!ReferenceEquals(gridUI, prevGridUI))
        {
            if (prevGridUI != null)
            {
                prevGridUI.OnEndDragOver();
            }

            if (gridUI != null)
            {
                gridUI.OnBeginDragOver(DraggingItem);
            }

            prevGridUI = gridUI;
        }

        if (gridUI != null)
        {
            gridUI.OnDragOver(mousePosition, DraggingItem);
        }
    }


    private void MoveItem(ItemLocation fromLocation, ItemLocation toLocation, ItemUI itemUI)
    {
        // 데이터 처리
        if (fromLocation != toLocation)
        {
            fromLocation.inventoryData.RemoveItem(fromLocation.gridID, itemUI.Data);
            toLocation.inventoryData.AddItemAtLocation(toLocation, itemUI.Data);
        }

        // UI 처리
        if (InventoryUIManager.TryGetUI(toLocation.inventoryData, out InventoryUI inventoryUI))
        {
            inventoryUI.PlaceItemUIAtLocation(toLocation, itemUI);
        }
        else
        {
            // 오브젝트 풀에 ItemUI 반환
        }
    }


    public void BeginItemDrag(ItemLocation fromLocation, ItemUI itemUI)
    {
        IsDragging = true;
        DraggingItem = itemUI;
        this.fromLocation = fromLocation;

        itemUI.transform.SetParent(transform);
        itemUI.transform.localPosition = Vector3.zero;
        itemUI.SetUITransparent(true);

        inventoryRaycaster.InitRaycaster();
        prevGridUI = null;
    }

    public void DropItem(ItemLocation toLocation)
    {
        IsDragging = false;

        DraggingItem.SetUITransparent(false);
        MoveItem(fromLocation, toLocation, DraggingItem);
        DraggingItem = null;

        if (prevGridUI != null)
        {
            prevGridUI.OnEndDragOver();
        }
    }

    public void CancelItemDrag()
    {
        DropItem(fromLocation);
    }
}
