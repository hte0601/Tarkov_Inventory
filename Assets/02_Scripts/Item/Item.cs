using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler
{
    private UnityEngine.UI.Image image;

    private ItemData itemData;

    private Vector3 lastPosition;
    private bool isRotated = false;
    private bool isDragging = false;

    // private int x_size;
    // private int y_size;

    private void Awake()
    {
        if (!TryGetComponent(out image))
        {
            Debug.LogError("오브젝트에서 특정 컴포넌트를 찾을 수 없음");
        }
    }


    public void BeginDrag()
    {
        isDragging = true;
        image.raycastTarget = false;
    }

    public void EndDrag()
    {
        isDragging = false;
        image.raycastTarget = true;
    }

    public void RotateItem()
    {
        if (isRotated)
        {
            isRotated = false;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            isRotated = true;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
            // transform.eulerAngles = new Vector3(0, 0, -90f);
        }
    }


    public void OnDrag(PointerEventData eventData) { }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        lastPosition = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryUIManager.instance.OnItemBeginDrag(eventData, this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        Debug.Log(eventData.dragging);
    }
}
