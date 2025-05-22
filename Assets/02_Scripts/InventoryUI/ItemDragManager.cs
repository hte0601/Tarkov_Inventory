using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragManager : MonoBehaviour
{
    public static ItemDragManager instance;

    private RectTransform canvasRectTransform;
    private Camera canvasWorldCamera;

    public ItemUI DraggingItem { get; private set; }
    public bool IsDragging { get; private set; }
    private bool wasItemRotated;

    private void Awake()
    {
        instance = this;
        DraggingItem = null;
        IsDragging = false;

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
        // late update로
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, Input.mousePosition, canvasWorldCamera, out Vector2 localPoint))
        {
            transform.localPosition = localPoint;
        }

        if (Input.GetKeyDown(KeyCode.R) && IsDragging)
        {
            DraggingItem.RotateItem();
        }
    }


    public void BeginItemDrag(ItemUI item)
    {
        IsDragging = true;
        wasItemRotated = item.IsRotated;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        DraggingItem = item;
    }

    public void DropItemToInventoryGrid(InventoryGridUI gridUI)
    {
        IsDragging = false;
        DraggingItem.transform.SetParent(gridUI.transform);
        DraggingItem = null;
    }

    public void CancleItemDrag(InventoryGridUI gridUI)
    {
        IsDragging = false;

        if (DraggingItem.IsRotated != wasItemRotated)
        {
            DraggingItem.RotateItem();
        }

        DraggingItem.transform.SetParent(gridUI.transform);
        DraggingItem = null;
    }
}
