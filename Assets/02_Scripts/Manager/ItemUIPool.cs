using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIPool : MonoBehaviour
{
    public static ItemUIPool instance;

    [SerializeField] private ItemUI itemUIPrefab;

    private Queue<ItemUI> itemUIQueue;
    private readonly int initialPoolSize = 30;
    private Transform cachedTransform;

    private void Awake()
    {
        instance = this;
        itemUIQueue = new Queue<ItemUI>();
        cachedTransform = transform;

        for (int i = 0; i < initialPoolSize; i++)
        {
            itemUIQueue.Enqueue(CreateItemUI());
        }
    }


    public ItemUI GetItemUI(ItemID itemID)
    {
        ItemData itemData = ItemFactory.CreateItemData(itemID);

        return GetItemUI(itemData);
    }

    public ItemUI GetItemUI(ItemData itemData)
    {
        ItemUI itemUI = itemUIQueue.Count == 0 ? CreateItemUI() : itemUIQueue.Dequeue();

        itemUI.SetupUI(itemData);
        itemUI.gameObject.SetActive(true);

        return itemUI;
    }

    public void ReleaseItemUI(ItemUI itemUI)
    {
        itemUI.gameObject.SetActive(false);
        itemUI.transform.SetParent(cachedTransform);
        itemUI.ResetUI();

        itemUIQueue.Enqueue(itemUI);
    }

    private ItemUI CreateItemUI()
    {
        ItemUI itemUI = Instantiate(itemUIPrefab, cachedTransform);
        itemUI.gameObject.SetActive(false);

        return itemUI;
    }
}
