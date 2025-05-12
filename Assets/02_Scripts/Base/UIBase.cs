using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OriginPoint
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    Pivot
}

public class UIBase : MonoBehaviour
{
    protected Canvas renderingCanvas;

    protected RectTransform rectTransform;

    protected virtual void Awake()
    {
        renderingCanvas = GetComponentInParent<Canvas>();
        if (!renderingCanvas)
        {
            Debug.LogError("Parent에서 Canvas 컴포넌트를 찾을 수 없음");
        }

        if (transform is RectTransform rect)
        {
            rectTransform = rect;
        }
        else
        {
            Debug.LogError("RectTransform을 가지고 있지 않음");
        }
    }

    protected bool ScreenPointToLocalPoint(
        Vector2 screenPoint, out Vector2 localPoint, OriginPoint origin = OriginPoint.Pivot)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, renderingCanvas.worldCamera, out localPoint))
        {
            return false;
        }

        if (origin == OriginPoint.Pivot)
        {
            return true;
        }

        // 기준점이 pivot이 아닌 경우
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        localPoint.x += rectTransform.pivot.x * width;
        localPoint.y += rectTransform.pivot.y * height;

        if (origin == OriginPoint.TopRight || origin == OriginPoint.BottomRight)
        {
            localPoint.x = width - localPoint.x;
        }

        if (origin == OriginPoint.TopLeft || origin == OriginPoint.TopRight)
        {
            localPoint.y = height - localPoint.y;
        }

        return true;
    }
}
