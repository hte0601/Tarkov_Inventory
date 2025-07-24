using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropIndicatorUI : UIBase
{
    private readonly Color32 greenColor = new(0, 255, 0, 48);
    private readonly Color32 redColor = new(255, 0, 0, 48);

    private Image indicatorImage;

    private RowColumn _index = new(-1, -1);
    private RowColumn _size = new(0, 0);

    public RowColumn Index
    {
        get
        {
            return _index;
        }
        private set
        {
            if (_index != value)
            {
                _index = value;

                Vector2 localPosition;
                localPosition.x = InventoryGridUI.SLOT_SIZE * value.col;
                localPosition.y = InventoryGridUI.SLOT_SIZE * value.row;
                localPosition.y *= -1f;

                rectTransform.localPosition = localPosition;
            }
        }
    }

    public RowColumn Size
    {
        get
        {
            return _size;
        }
        private set
        {
            if (_size != value)
            {
                _size = value;

                ItemSizeData itemSizeData = new(value.col, value.row);
                rectTransform.sizeDelta = ItemUI.CalcItemUIObjectSize(itemSizeData);
            }
        }
    }


    protected override void Awake()
    {
        base.Awake();

        if (TryGetComponent(out indicatorImage))
        {
            indicatorImage.enabled = false;

            Index = new(0, 0);
            Size = new(1, 1);
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }
    }


    public void BeginDragOver(RowColumn itemSize)
    {
        Size = itemSize;

        indicatorImage.enabled = true;
    }

    public void EndDragOver()
    {
        indicatorImage.enabled = false;
    }


    public void SetIndicator(RowColumn index, RowColumn size, bool canDrop)
    {
        Index = index;
        Size = size;

        indicatorImage.color = canDrop ? greenColor : redColor;
    }
}
