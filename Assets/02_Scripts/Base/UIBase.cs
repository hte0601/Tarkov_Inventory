using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    private Canvas cachedRenderingCanvas = null;
    private bool wasTransformParentChanged = true;

    protected RectTransform rectTransform;

    protected Canvas RenderingCanvas
    {
        get
        {
            if (wasTransformParentChanged)
            {
                wasTransformParentChanged = false;

                cachedRenderingCanvas = GetComponentInParent<Canvas>();
                if (cachedRenderingCanvas == null)
                {
                    Debug.LogError("InParent에서 Canvas 컴포넌트를 찾을 수 없음");
                }
            }

            return cachedRenderingCanvas;
        }
    }


    protected virtual void Awake()
    {
        if (transform is RectTransform rect)
        {
            rectTransform = rect;
        }
        else
        {
            Debug.LogError("게임오브젝트의 Transform이 RectTransform이 아님");
        }
    }

    private void OnTransformParentChanged()
    {
        wasTransformParentChanged = true;
    }


    protected bool ScreenPointToLocalPoint(Vector2 screenPoint, out Vector2 localPoint)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, RenderingCanvas.worldCamera, out localPoint);
    }
}
