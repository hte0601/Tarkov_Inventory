using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropIndicatorUI : UIBase
{
    private readonly Color32 greenColor = new(0, 255, 0, 48);
    private readonly Color32 redColor = new(255, 0, 0, 48);

    private Image indicatorImage;

    private RowColumn _gridIndex;
    private RowColumn _indicatorSize;

    private RowColumn GridIndex
    {
        get
        {
            return _gridIndex;
        }
        set
        {
            _gridIndex = value;

            Vector2 localPosition;
            localPosition.x = InventoryGridUI.SLOT_SIZE * value.col;
            localPosition.y = InventoryGridUI.SLOT_SIZE * value.row;
            localPosition.y *= -1f;

            rectTransform.localPosition = localPosition;
        }
    }

    private RowColumn IndicatorSize
    {
        get
        {
            return _indicatorSize;
        }
        set
        {
            _indicatorSize = value;

            ItemSizeData itemSizeData = new(value.col, value.row);
            rectTransform.sizeDelta = ItemUI.CalcItemUIObjectSize(itemSizeData);
        }
    }


    protected override void Awake()
    {
        base.Awake();

        if (TryGetComponent(out indicatorImage))
        {
            indicatorImage.enabled = false;

            GridIndex = new RowColumn(0, 0);
            IndicatorSize = new RowColumn(1, 1);
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }
    }


    public void BeginDragOver()
    {
        indicatorImage.enabled = true;
    }

    public void EndDragOver()
    {
        indicatorImage.enabled = false;
    }


    public void SetIndicator(RowColumn gridIndex, RowColumn indicatorSize, bool canDrop)
    {
        if (GridIndex != gridIndex)
        {
            GridIndex = gridIndex;
        }

        if (IndicatorSize != indicatorSize)
        {
            IndicatorSize = indicatorSize;
        }

        indicatorImage.color = canDrop ? greenColor : redColor;
    }
}
