using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    protected bool ScreenPointToLocalPoint(Vector2 screenPoint, out Vector2 localPoint)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, renderingCanvas.worldCamera, out localPoint);
    }
}
