using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ItemData data;  //

    private StashInventoryUI stashUI;
    public RowColumn Index { get; private set; }

    private RowColumn _gridSize;
    private Vector2 _topLeftSlotPoint;
    private bool _isRotated;

    public RowColumn GridSize
    {
        get { return _gridSize; }
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

            _gridSize.row = value ? data.GridSize.width : data.GridSize.height;
            _gridSize.col = value ? data.GridSize.height : data.GridSize.width;

            _topLeftSlotPoint.x = -(_gridSize.col - 1) / 2f * InventoryUI.SLOT_SIZE;
            _topLeftSlotPoint.y = (_gridSize.row - 1) / 2f * InventoryUI.SLOT_SIZE;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        // 임시 코드
        if (!transform.parent.TryGetComponent(out stashUI))
        {
            Debug.LogError("부모 오브젝트에서 StashInventoryUI 컴포넌트를 찾을 수 없음");
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

    public void UpdateLocation(StashInventoryUI stashUI, RowColumn index)
    {
        this.stashUI = stashUI;
        Index = index;
    }


    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (stashUI)
        {
            stashUI.OnItemBeginDrag(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (stashUI)
        {
            stashUI.OnItemEndDrag(this);
        }
    }
}
