using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager instance;

    [SerializeField] private MousePointerUI mousePointerUI;


    private bool isDragging = false;
    private Item draggingItem = null;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isDragging)
        {
            draggingItem.RotateItem();
        }
    }

    public void OnInventoryDrop(PointerEventData eventData, InventoryUI inventoryUI)
    {
        if (isDragging)
        {
            isDragging = false;
            draggingItem.transform.SetParent(inventoryUI.transform);
            draggingItem.EndDrag();
            draggingItem = null;
        }
    }

    public void OnItemBeginDrag(PointerEventData eventData, Item item)
    {
        if (!isDragging)
        {
            isDragging = true;
            item.BeginDrag();
            item.transform.SetParent(mousePointerUI.transform);
            item.transform.localPosition = Vector3.zero;
            draggingItem = item;
        }
    }
}
