using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ItemData data;  // 임시 SerializeField

    private InventoryGridUI gridUI;
    public RowColumn Index { get; private set; }

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

            _size.row = value ? data.ItemSize.width : data.ItemSize.height;
            _size.col = value ? data.ItemSize.height : data.ItemSize.width;

            _topLeftSlotPoint.x = -(_size.col - 1) / 2f * InventoryGridUI.SLOT_SIZE;
            _topLeftSlotPoint.y = (_size.row - 1) / 2f * InventoryGridUI.SLOT_SIZE;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        // 임시 코드
        if (!transform.parent.TryGetComponent(out gridUI))
        {
            Debug.LogError("부모 오브젝트에서 InventoryGridUI 컴포넌트를 찾을 수 없음");
        }

        IsRotated = false;
        //
    }


    // public void InitUI(ItemData data)
    // {
    //     this.data = data;
    // }


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
