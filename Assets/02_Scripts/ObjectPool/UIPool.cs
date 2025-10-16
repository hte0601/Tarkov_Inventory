using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableUI
{
    public void ResetUI();
}

public class UIPool<T> where T : MonoBehaviour, IPoolableUI
{
    private readonly T uiPrefab;
    private readonly Queue<T> uiQueue;
    private readonly RectTransform poolTransform;

    public UIPool(T uiPrefab, RectTransform poolTransform, int initialPoolSize = 0)
    {
        this.uiPrefab = uiPrefab;
        this.poolTransform = poolTransform;
        uiQueue = new Queue<T>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            uiQueue.Enqueue(CreateUI());
        }
    }


    public void ClearPool()
    {
        while (uiQueue.Count > 0)
        {
            Object.Destroy(uiQueue.Dequeue().gameObject);
        }
    }


    public T GetUI(bool isActive = true)
    {
        T ui = uiQueue.Count == 0 ? CreateUI() : uiQueue.Dequeue();
        ui.gameObject.SetActive(isActive);

        return ui;
    }

    public void ReleaseUI(T ui)
    {
        ui.gameObject.SetActive(false);
        ui.transform.SetParent(poolTransform, false);
        ui.ResetUI();

        uiQueue.Enqueue(ui);
    }

    protected T CreateUI()
    {
        T ui = Object.Instantiate(uiPrefab, poolTransform, false);
        ui.gameObject.SetActive(false);

        return ui;
    }
}
