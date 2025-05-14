using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : UIBase, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ItemData data;  //

    private StashInventoryUI stashUI;
    private RowColumn index;
    private bool _isRotated;

    [HideInInspector] public Vector2 originPoint;  //

    public bool IsRotated
    {
        get { return _isRotated; }
        private set
        {
            if (value)
            {
                _isRotated = true;
                transform.rotation = Quaternion.Euler(0, 0, -90f);
                originPoint.x = -(data.sizeY - 1) / 2f * InventoryUI.SLOT_SIZE;  // 캐싱?
                originPoint.y = (data.sizeX - 1) / 2f * InventoryUI.SLOT_SIZE;
            }
            else
            {
                _isRotated = false;
                transform.rotation = Quaternion.identity;
                originPoint.x = -(data.sizeX - 1) / 2f * InventoryUI.SLOT_SIZE;
                originPoint.y = (data.sizeY - 1) / 2f * InventoryUI.SLOT_SIZE;
            }
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

        index.row = 0;
        index.col = 0;
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

    public void MoveItemTo(StashInventoryUI stashUI, RowColumn index)
    {
        this.stashUI = stashUI;
        this.index = index;
    }


    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (stashUI)
        {
            stashUI.OnItemBeginDrag(this, index);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (stashUI)
        {
            stashUI.OnItemEndDrag(this, index);
        }
    }
}
