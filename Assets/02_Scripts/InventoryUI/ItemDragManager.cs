using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragManager : MonoBehaviour
{
    public static ItemDragManager instance;

    private RectTransform canvasRectTransform;
    private Camera canvasWorldCamera;

    private ItemUI draggingItem = null;
    private bool wasItemRotated;
    private bool _isDragging = false;

    public bool IsDragging
    {
        get { return _isDragging; }
        private set { _isDragging = value; }
    }


    private void Awake()
    {
        instance = this;

        Canvas renderingCanvas = GetComponentInParent<Canvas>();
        if (renderingCanvas)
        {
            canvasWorldCamera = renderingCanvas.worldCamera;
            canvasRectTransform = renderingCanvas.transform as RectTransform;
        }
        else
        {
            Debug.LogError("Parent에서 Canvas 컴포넌트를 찾을 수 없음");
        }
    }

    private void Update()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, Input.mousePosition, canvasWorldCamera, out Vector2 localPoint))
        {
            transform.localPosition = localPoint;
        }

        if (Input.GetKeyDown(KeyCode.R) && IsDragging)
        {
            draggingItem.RotateItem();
        }
    }


    public ItemUI GetDraggingItem()
    {
        return draggingItem;
    }

    public void BeginItemDrag(ItemUI item)
    {
        IsDragging = true;
        wasItemRotated = item.IsRotated;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        draggingItem = item;
    }

    public void DropItemToInventory(InventoryUI inventoryUI)
    {
        IsDragging = false;
        draggingItem.transform.SetParent(inventoryUI.transform);
        draggingItem = null;
    }

    public void CancleItemDrag(InventoryUI inventoryUI)
    {
        IsDragging = false;

        if (draggingItem.IsRotated != wasItemRotated)
        {
            draggingItem.RotateItem();
        }

        draggingItem.transform.SetParent(inventoryUI.transform);
        draggingItem = null;
    }
}
