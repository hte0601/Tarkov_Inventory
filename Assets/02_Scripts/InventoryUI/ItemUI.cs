using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ItemID itemID;
    public ItemData Data { get; private set; }

    private InventoryGridUI gridUI;
    public RowColumn Index { get; private set; }

    private Image itemImage;
    private RowColumn _size;
    private Vector2 _topLeftSlotPoint;
    private bool _isRotated;

    public RowColumn Size
    {
        get { return _size; }
    }

    public Vector2 TopLeftSlotPoint
    {
        get { return _topLeftSlotPoint; }
    }

    public bool IsRotated
    {
        get { return _isRotated; }
        private set
        {
            _isRotated = value;
            transform.rotation = value ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;

            _size.row = value ? Data.ItemSize.width : Data.ItemSize.height;
            _size.col = value ? Data.ItemSize.height : Data.ItemSize.width;

            _topLeftSlotPoint.x = -(_size.col - 1) / 2f * InventoryGridUI.SLOT_SIZE;
            _topLeftSlotPoint.y = (_size.row - 1) / 2f * InventoryGridUI.SLOT_SIZE;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        if (!TryGetComponent(out itemImage))
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }

        // 임시 코드
        Data = ItemFactory.CreateItemData(itemID);

        gridUI = transform.GetComponentInParent<InventoryGridUI>();
        if (!gridUI)
        {
            Debug.LogError("InParent에서 InventoryGridUI 컴포넌트를 찾을 수 없음");
        }

        IsRotated = false;
        //
    }


    public void SetItemImageAlpha(byte alpha)
    {
        itemImage.color = new Color32(255, 255, 255, alpha);
    }


    public void RotateItem()
    {
        IsRotated = !IsRotated;
    }

    public void UpdateLocation(InventoryGridUI gridUI, RowColumn index)
    {
        this.gridUI = gridUI;
        Index = index;
    }


    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (gridUI)
        {
            gridUI.OnItemBeginDrag(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (gridUI)
        {
            gridUI.OnItemEndDrag(this);
        }
    }
}
