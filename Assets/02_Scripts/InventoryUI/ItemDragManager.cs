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

    public ItemUI DraggingItem { get; private set; }
    public bool IsDragging { get; private set; }
    private bool wasItemRotated;

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
            DraggingItem.RotateItem();
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


    public void BeginItemDrag(ItemUI item)
    {
        IsDragging = true;
        wasItemRotated = item.IsRotated;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.SetItemImageAlpha(224);
        DraggingItem = item;

        inventoryRaycaster.InitRaycaster();
        prevGridUI = null;
    }

    public void DropItemToInventoryGrid(InventoryGridUI gridUI)
    {
        IsDragging = false;
        DraggingItem.SetItemImageAlpha(255);
        DraggingItem.transform.SetParent(gridUI.ItemListTransform);
        DraggingItem = null;

        gridUI.OnEndDragOver();  // 인벤토리가 있는 아이템 아이콘에 드래그 할 때는 다를 듯
    }

    public void CancleItemDrag(InventoryGridUI gridUI)
    {
        IsDragging = false;

        if (DraggingItem.IsRotated != wasItemRotated)
        {
            DraggingItem.RotateItem();
        }

        DraggingItem.SetItemImageAlpha(255);
        DraggingItem.transform.SetParent(gridUI.ItemListTransform);
        DraggingItem = null;

        if (prevGridUI != null)
        {
            prevGridUI.OnEndDragOver();
        }
    }
}
