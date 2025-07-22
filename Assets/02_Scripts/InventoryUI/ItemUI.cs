using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
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

            transform.rotation = value ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;
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


    private void SetUIObjectSize(ItemSizeData itemSize)
    {
        Vector2 size;
        size.x = InventoryGridUI.SLOT_SIZE * itemSize.width + 1;
        size.y = InventoryGridUI.SLOT_SIZE * itemSize.height + 1;

        rectTransform.sizeDelta = size;
    }


    public void SetupUI(ItemData itemData)
    {
        Data = itemData;

        SetUIObjectSize(Data.ItemCurrentSize);
        IsUIRotated = Data.IsRotated;

        ResourceManager.TryGetItemIcon(Data.IconPath, out Sprite iconSprite);
        itemImage.sprite = iconSprite;
    }

    public void ResetUI()
    {
        Data = null;
        parentGridUI = null;
        itemImage.sprite = null;

        transform.rotation = Quaternion.identity;
        _isUIRotated = false;
        UISize = new RowColumn(1, 1);
        TopLeftSlotPoint = new Vector2(0, 0);
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
