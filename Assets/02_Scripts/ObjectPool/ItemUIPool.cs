using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIPool : MonoBehaviour
{
    public static ItemUIPool instance;

    [SerializeField] private ItemUI itemUIPrefab;

    private UIPool<ItemUI> itemUIPool;
    private readonly int initialPoolSize = 30;

    private void Awake()
    {
        instance = this;
        itemUIPool = new UIPool<ItemUI>(itemUIPrefab, transform as RectTransform, initialPoolSize);
    }


    public ItemUI GetItemUI(ItemData itemData)
    {
        ItemUI itemUI = itemUIPool.GetUI(false);

        itemUI.SetupUI(itemData);
        itemUI.gameObject.SetActive(true);

        return itemUI;
    }

    public void ReleaseItemUI(ItemUI itemUI)
    {
        itemUIPool.ReleaseUI(itemUI);
    }
}
