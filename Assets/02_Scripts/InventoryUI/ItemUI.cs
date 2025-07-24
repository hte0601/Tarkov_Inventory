using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private static readonly Dictionary<RowColumn, Vector2> cachedTopLeftCellOffset = new();

    public static Vector2 CalcItemUIObjectSize(ItemSizeData itemSizeData)
    {
        Vector2 sizeDelta;
        sizeDelta.x = InventoryGridUI.SLOT_SIZE * itemSizeData.width + 1;
        sizeDelta.y = InventoryGridUI.SLOT_SIZE * itemSizeData.height + 1;

        return sizeDelta;
    }


    public ItemData Data { get; private set; }

    public InventoryGridUI parentGridUI;

    private Image itemImage;
    public RowColumn UISize { get; private set; }
    public Vector2 TopLeftSlotPoint { get; private set; }
    private bool _isUIRotated;

    public bool IsUIRotated
    {
        get { return _isUIRotated; }
        set
        {
            _isUIRotated = value;

            rectTransform.rotation = value ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;
            UISize = Data.GetItemSize(value);

            Vector2 point;
            point.x = -(UISize.col - 1) / 2f * InventoryGridUI.SLOT_SIZE;
            point.y = (UISize.row - 1) / 2f * InventoryGridUI.SLOT_SIZE;
            TopLeftSlotPoint = point;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        if (!TryGetComponent(out itemImage))
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }
    }


    public void SetupUI(ItemData itemData)
    {
        Data = itemData;

        rectTransform.sizeDelta = CalcItemUIObjectSize(Data.ItemSizeData);
        IsUIRotated = Data.IsRotated;

        ResourceManager.TryGetItemIcon(Data.IconPath, out Sprite iconSprite);
        itemImage.sprite = iconSprite;
    }

    public void ResetUI()
    {
        Data = null;
        parentGridUI = null;

        itemImage.sprite = null;
        rectTransform.rotation = Quaternion.identity;
        rectTransform.sizeDelta = CalcItemUIObjectSize(new ItemSizeData(1, 1));

        _isUIRotated = false;
        UISize = new RowColumn(1, 1);
        TopLeftSlotPoint = new Vector2(0, 0);
    }

    public void SetItemImageEnabled(bool enabled)
    {
        itemImage.enabled = enabled;
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

    public Sprite GetItemImageSprite()
    {
        return itemImage.sprite;
    }


    public void SetUITransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            itemImage.color = new Color32(255, 255, 255, 224);
        }
        else
        {
            itemImage.color = new Color32(255, 255, 255, 255);
        }
    }

    public void RotateItemUI()
    {
        IsUIRotated = !IsUIRotated;
    }


    // EventSystem Handler 구현
    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && parentGridUI != null)
        {
            parentGridUI.OnItemBeginDrag(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left
            && ItemDragManager.instance.IsDragging
            && ReferenceEquals(this, ItemDragManager.instance.DraggingItem))
        {
            ItemDragManager.instance.CancelItemDrag();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && parentGridUI != null)
        {
            parentGridUI.OnDrop(eventData);
        }
    }
}
