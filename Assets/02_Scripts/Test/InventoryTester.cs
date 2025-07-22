using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventoryUI stashUI;
    [SerializeField] private InventoryUI shopUI;
    [SerializeField] private InventoryUI aacpcUI;

    private readonly Dictionary<ItemID, int> itemListToCreate = new()
    {
        { ItemID.LEDX, 3 },
        { ItemID.PhysicalBitcoin, 3 },
        { ItemID.GraphicsCard, 3 },
        { ItemID.Tetriz, 3 },
        { ItemID.IntelligenceFolder, 3 },
        { ItemID.PhasedArrayElement, 3 },
        { ItemID.CatFigurine, 3 },

        { ItemID.MilitaryBattery, 1 },
        { ItemID.FierceBlowSledgehammer, 1 },
    };


    private void Start()
    {
        if (stashUI != null)
        {
            stashUI.SetupUI(new InventoryData(stashUI.inventorySize));
        }

        if (shopUI != null)
        {
            shopUI.SetupUI(new InventoryData(shopUI.inventorySize));

            foreach (var item in itemListToCreate)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    AddItemToInventory(shopUI, item.Key, out _);
                }
            }
        }

        if (aacpcUI != null && shopUI != null)
        {
            if (AddItemToInventory(shopUI, ItemID.AACPC, out ItemUI itemUI)
                && itemUI.Data is IContainableItem itemData)
            {
                aacpcUI.SetupUI(itemData.InnerInventoryData);
            }
            else
            {
                aacpcUI.SetupUI(new InventoryData(aacpcUI.inventorySize));
            }
        }
    }


    private bool AddItemToInventory(InventoryUI inventoryUI, ItemID itemID, out ItemUI itemUI)
    {
        ItemData itemData = ItemFactory.CreateItemData(itemID);

        if (inventoryUI.Data.TryFindLocationToAddItem(itemData, out ItemLocation location))
        {
            itemUI = ItemUIPool.instance.GetItemUI(itemData);
            ItemDragManager.instance.AddItemAtLocation(location, itemUI);

            return true;
        }
        else
        {
            Debug.Log("(test) 인벤토리에 아이템을 추가 할 자리가 없음");

            itemUI = null;
            return false;
        }
    }
}
