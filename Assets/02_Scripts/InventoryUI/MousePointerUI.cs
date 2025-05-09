using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerUI : MonoBehaviour
{
    private RectTransform canvasRectTransform;
    private Camera canvasWorldCamera;

    private Vector2 localPoint;


    private void Awake()
    {
        if (transform.parent.TryGetComponent(out Canvas canvas))
        {
            canvasWorldCamera = canvas.worldCamera;
            canvasRectTransform = canvas.transform as RectTransform;
        }
        else
        {
            Debug.LogError("부모 오브젝트에서 특정 컴포넌트를 찾을 수 없음");
        }
    }

    private void Update()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, Input.mousePosition, canvasWorldCamera, out localPoint))
        {
            transform.localPosition = localPoint;
        }
    }
}
