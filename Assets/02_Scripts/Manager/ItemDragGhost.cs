using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragGhost : UIBase
{
    private Image dragGhostImage;

    protected override void Awake()
    {
        base.Awake();

        if (TryGetComponent(out dragGhostImage))
        {
            dragGhostImage.enabled = false;
            dragGhostImage.color = new Color32(255, 255, 255, 224);
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없음");
        }
    }


    public void BeginItemDrag(ItemDragContext dragContext)
    {
        rectTransform.sizeDelta = ItemUI.CalcItemUIObjectSize(dragContext.ItemData.ItemSizeData);
        RotateImage(dragContext.IsItemUIRotated);
        dragGhostImage.sprite = dragContext.DraggingItemUI.GetItemImageSprite();
        dragGhostImage.enabled = true;
    }

    public void EndItemDrag()
    {
        dragGhostImage.enabled = false;
        dragGhostImage.sprite = null;
        RotateImage(false);
        rectTransform.sizeDelta = ItemUI.CalcItemUIObjectSize(new ItemSizeData(1, 1));
    }

    public void RotateImage(bool isRotated)
    {
        rectTransform.rotation = isRotated ? Quaternion.Euler(0, 0, -90f) : Quaternion.identity;
    }
}
