using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StashUI : InventoryUI, IDropHandler
{
    private InventoryUIManager inventoryUIManager;

    private void Awake()
    {
        if (!transform.parent.TryGetComponent(out inventoryUIManager))
        {
            Debug.LogError("부모 오브젝트에서 특정 컴포넌트를 찾을 수 없음");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryUIManager.instance.OnInventoryDrop(eventData, this);
        }
    }
}
