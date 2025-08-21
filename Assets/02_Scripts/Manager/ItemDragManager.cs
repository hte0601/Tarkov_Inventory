using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragManager : MonoBehaviour
{
    public static ItemDragManager instance;

    [SerializeField] private ItemDragGhost dragGhost;
    public readonly ItemDragContext dragContext = new();

    private InventoryRaycaster inventoryRaycaster;
    private InventoryGridUI prevGridUI;

    private Transform cachedTransform;
    private RectTransform canvasRectTransform;
    private Camera canvasWorldCamera;

    private void Awake()
    {
        instance = this;

        inventoryRaycaster = new InventoryRaycaster();
        prevGridUI = null;
        cachedTransform = transform;

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
        if (!dragContext.IsDragging)
        {
            return;
        }

        // 오브젝트를 마우스 위치로 이동
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, Input.mousePosition, canvasWorldCamera, out Vector2 localPoint))
        {
            cachedTransform.localPosition = localPoint;
        }

        // 아이템 회전 입력 처리
        if (Input.GetKeyDown(KeyCode.R))
        {
            dragContext.RotateItemUI();
            dragGhost.RotateImage(dragContext.IsItemUIRotated);
        }

        // 아이템 DragOver 처리
        Vector2 mousePosition = Input.mousePosition;
        inventoryRaycaster.RaycastGridUI(mousePosition, out InventoryGridUI gridUI);

        if (!ReferenceEquals(gridUI, prevGridUI))
        {
            if (prevGridUI != null)
            {
                prevGridUI.OnDragOverEnd();
            }

            if (gridUI != null)
            {
                gridUI.OnDragOverBegin();
            }

            prevGridUI = gridUI;
        }

        if (gridUI != null)
        {
            gridUI.OnDragOver(dragContext, mousePosition);
        }
    }


    public void BeginItemDrag(ItemUI itemUI, ItemLocation fromLocation)
    {
        itemUI.SetItemImageEnabled(false);
        dragContext.BeginItemDrag(itemUI, fromLocation);
        dragGhost.BeginItemDrag(dragContext);

        inventoryRaycaster.InitRaycaster();
        prevGridUI = null;
    }

    public void EndItemDrag(ItemLocation toLocation)
    {
        if (prevGridUI != null)
        {
            prevGridUI.OnDragOverEnd();
        }

        dragGhost.EndItemDrag();
        dragContext.DraggingItemUI.SetItemImageEnabled(true);

        InventoryDataManager.instance.MoveItemData(dragContext.ItemData, dragContext.FromLocation, toLocation);

        dragContext.EndItemDrag();
    }

    public void CancelItemDrag()
    {
        if (prevGridUI != null)
        {
            prevGridUI.OnDragOverEnd();
        }

        dragGhost.EndItemDrag();
        dragContext.DraggingItemUI.SetItemImageEnabled(true);
        dragContext.EndItemDrag();
    }
}
