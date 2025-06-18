using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryRaycaster
{
    private EventSystem eventSystem;
    private PointerEventData eventData;
    private List<RaycastResult> raycastResultList;

    private GameObject prevTopUIObj;
    private InventoryGridUI cachedGridUI;

    public InventoryRaycaster()
    {
        raycastResultList = new();
    }


    public void InitRaycaster()
    {
        eventSystem = EventSystem.current;
        eventData = new(eventSystem);

        prevTopUIObj = null;
        cachedGridUI = null;
    }

    public bool RaycastGridUI(Vector2 eventPosition, out InventoryGridUI gridUI)
    {
        eventData.position = eventPosition;
        eventSystem.RaycastAll(eventData, raycastResultList);

        int count = raycastResultList.Count;
        int i = 0;

        //  ItemUI는 무시
        while (i < count && raycastResultList[i].gameObject.CompareTag("ItemUI"))
        {
            i++;
        }

        // Ray에 hit 된 UI가 하나도 없거나 전부 ItemUI인 경우
        if (count == 0 || count <= i)
        {
            prevTopUIObj = null;
            cachedGridUI = null;

            gridUI = null;
            return false;
        }

        GameObject topUIObj = raycastResultList[i].gameObject;

        if (ReferenceEquals(topUIObj, prevTopUIObj))
        {
            gridUI = cachedGridUI;
            return gridUI != null;
        }

        // topUIObj와 prevTopUIObj가 다른 경우
        prevTopUIObj = topUIObj;

        if (topUIObj.CompareTag("GridUI"))
        {
            if (topUIObj.TryGetComponent(out cachedGridUI))
            {
                gridUI = cachedGridUI;
                return true;
            }
            else
            {
                Debug.LogError("GridUI 태그가 있는 게임오브젝트에서 GridUI 컴포넌트를 찾을 수 없음");

                gridUI = null;
                return false;
            }
        }
        // 최상단 UI가 GridUI가 아닌 경우
        else
        {
            cachedGridUI = null;

            gridUI = null;
            return false;
        }
    }
}
