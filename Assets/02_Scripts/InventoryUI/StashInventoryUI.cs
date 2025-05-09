using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StashInventoryUI : InventoryUI, IDropHandler
{
    private InventoryCanvas inventoryCanvas;

    private void Awake()
    {
        if (!transform.parent.TryGetComponent(out inventoryCanvas))
        {
            Debug.LogError("부모 오브젝트에서 특정 컴포넌트를 찾을 수 없음");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryCanvas.instance.OnInventoryDrop(eventData, this);
        }
    }
}
