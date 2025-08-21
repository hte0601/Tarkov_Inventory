using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private InventoryUI stashUI;
    [SerializeField] private InventoryUI shopUI;
    [SerializeField] private InventoryUI aacpcUI;

    InventoryUIController uiController;
    InventoryDataManager dataManager;

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


    private void Awake()
    {
        uiController = InventoryUIController.instance;
        dataManager = InventoryDataManager.instance;
    }

    private void Start()
    {
        if (stashUI != null)
        {
            uiController.OpenUI(new InventoryData(stashUI.inventorySize), stashUI);
        }

        if (shopUI != null)
        {
            uiController.OpenUI(new InventoryData(shopUI.inventorySize), shopUI);

            foreach (var item in itemListToCreate)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    TryAddItemToInventory(shopUI, item.Key);
                }
            }
        }

        if (aacpcUI != null && stashUI != null)
        {
            ItemData aacpcItemData = ItemFactory.CreateItemData(ItemID.AACPC);

            if (TryAddItemToInventory(stashUI, aacpcItemData)
                && aacpcItemData is IContainableItemData aacpcRigData)
            {
                uiController.OpenUI(aacpcRigData.InnerInventoryData, aacpcUI);
            }
            else
            {
                uiController.OpenUI(new InventoryData(aacpcUI.inventorySize), aacpcUI);
            }
        }
    }


    private bool TryAddItemToInventory(InventoryUI inventoryUI, ItemID itemID)
    {
        ItemData itemData = ItemFactory.CreateItemData(itemID);

        if (inventoryUI.Data.TryFindLocationToAddItem(itemData, out ItemLocation addLocation))
        {
            dataManager.AddItemData(itemData, addLocation);

            return true;
        }
        else
        {
            Debug.Log("(test) 인벤토리에 아이템을 추가 할 자리가 없음");

            return false;
        }
    }

    private bool TryAddItemToInventory(InventoryUI inventoryUI, ItemData itemData)
    {
        if (inventoryUI.Data.TryFindLocationToAddItem(itemData, out ItemLocation addLocation))
        {
            dataManager.AddItemData(itemData, addLocation);

            return true;
        }
        else
        {
            Debug.Log("(test) 인벤토리에 아이템을 추가 할 자리가 없음");

            return false;
        }
    }
}
