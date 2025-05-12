using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StashInventoryUI : InventoryUI, IDropHandler, IPointerDownHandler
{
    protected override void Awake()
    {
        base.Awake();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryCanvas.instance.OnInventoryDrop(eventData, this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RowColumn index = EventPosToInventoryIndex(eventData.position);
        Debug.LogFormat("({0},{1})", index.row, index.col);
    }
}
