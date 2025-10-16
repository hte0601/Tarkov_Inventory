using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : UIBase, IPoolableUI, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private static readonly Dictionary<RowColumn, Vector2> cachedTopLeftCellOffset = new();

    public static Vector2 CalcItemUIObjectSize(ItemSizeData itemSizeData)
    {
        Vector2 sizeDelta;
        sizeDelta.x = InventoryGridUI.SLOT_SIZE * itemSizeData.width + 1;
        sizeDelta.y = InventoryGridUI.SLOT_SIZE * itemSizeData.height + 1;

        return sizeDelta;
    }


    public event Action<ItemUI, PointerEventData> OnDoubleClick;
    public event Action<ItemUI, PointerEventData> OnDragBegin;
    public event Action<ItemUI, PointerEventData> OnDragEnd;
    public event Action<ItemUI, PointerEventData> OnDropped;

    public ItemData Data { get; private set; }
    private Image itemImage;
    private DoubleClickDetector clickDetector;

    protected override void Awake()
    {
        base.Awake();

        clickDetector = new DoubleClickDetector();

        if (!TryGetComponent(out itemImage))
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }
    }


    public void SetupUI(ItemData itemData)
    {
        Data = itemData;

        rectTransform.sizeDelta = CalcItemUIObjectSize(Data.ItemSizeData);
        rectTransform.rotation = Data.IsItemRotated ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;

        ResourceManager.TryGetItemIcon(Data.IconPath, out Sprite iconSprite);
        itemImage.sprite = iconSprite;
    }

    public void ResetUI()
    {
        Data = null;
        itemImage.sprite = null;
        rectTransform.rotation = Quaternion.identity;
        rectTransform.sizeDelta = CalcItemUIObjectSize(new ItemSizeData(1, 1));
    }


    public void SetItemImageEnabled(bool enabled)
    {
        itemImage.enabled = enabled;
    }

    public Sprite GetItemImageSprite()
    {
        return itemImage.sprite;
    }

    public Vector2 GetTopLeftCellOffset(bool isItemUIRotated)
    {
        RowColumn itemSize = Data.GetItemSize(isItemUIRotated);

        if (!cachedTopLeftCellOffset.TryGetValue(itemSize, out Vector2 offset))
        {
            offset.x = -(itemSize.col - 1) / 2f * InventoryGridUI.SLOT_SIZE;
            offset.y = (itemSize.row - 1) / 2f * InventoryGridUI.SLOT_SIZE;

            cachedTopLeftCellOffset.Add(itemSize, offset);
        }

        return offset;
    }


    // EventSystem Handler 구현
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && clickDetector.IsDoubleClick())
        {
            OnDoubleClick?.Invoke(this, eventData);
        }
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnDragBegin?.Invoke(this, eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnDragEnd?.Invoke(this, eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnDropped?.Invoke(this, eventData);
        }
    }
}
