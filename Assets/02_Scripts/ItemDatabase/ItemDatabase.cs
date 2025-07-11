using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static readonly Dictionary<ItemID, string> iconPathDict;
    private static readonly Dictionary<ItemID, ItemInfo> itemInfoDict;

    public static bool TryGetItemInfo(ItemID itemID, out ItemInfo itemInfo)
    {
        if (itemInfoDict.TryGetValue(itemID, out itemInfo))
        {
            return true;
        }
        else
        {
            Debug.LogError("itemID에 해당하는 itemInfo를 찾을 수 없음");

            return false;
        }
    }


    static ItemDatabase()
    {
        iconPathDict = new()
        {
            { ItemID.GraphicsCard, "Item_icon/Graphics_card_icon" },
            { ItemID.Tetriz, "Item_icon/Tetriz_icon" },
            { ItemID.PhasedArrayElement, "Item_icon/Phased_array_element_icon" },
            { ItemID.MilitaryPowerFilter, "Item_icon/Military_power_filter_icon" },

            { ItemID.MilitaryBattery, "Item_icon/Military_battery_icon" },

            { ItemID.LEDX, "Item_icon/LEDX_icon" },

            { ItemID.FierceBlowSledgehammer, "Item_icon/Fierce_blow_sledgehammer_icon" },

            { ItemID.PhysicalBitcoin, "Item_icon/Physical_bitcoin_icon" },
            { ItemID.CatFigurine, "Item_icon/Cat_figurine_icon" },

            { ItemID.BerkutBackpack, "Item_icon/Berkut_backpack_icon" },

            { ItemID.AACPC, "Item_icon/AACPC_icon" },

            { ItemID.IntelligenceFolder, "Item_icon/Intelligence_folder_icon" },
        };

        itemInfoDict = new()
        {
            { ItemID.GraphicsCard, new ItemInfo(ItemID.GraphicsCard,
                iconPathDict[ItemID.GraphicsCard],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics),
                new(2, 1)) },

            { ItemID.Tetriz, new ItemInfo(ItemID.Tetriz,
                iconPathDict[ItemID.Tetriz],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics),
                new(1, 2)) },

            { ItemID.PhasedArrayElement, new ItemInfo(ItemID.PhasedArrayElement,
                iconPathDict[ItemID.PhasedArrayElement],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics),
                new(2, 2)) },

            { ItemID.MilitaryPowerFilter, new ItemInfo(ItemID.MilitaryPowerFilter,
                iconPathDict[ItemID.MilitaryPowerFilter],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics),
                new(1, 1)) },


            { ItemID.MilitaryBattery, new ItemInfo(ItemID.MilitaryBattery,
                iconPathDict[ItemID.MilitaryBattery],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.EnergyElements),
                new(4, 2)) },


            { ItemID.LEDX, new ItemInfo(ItemID.LEDX,
                iconPathDict[ItemID.LEDX],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.MedicalSupplies),
                new(1, 1)) },


            { ItemID.FierceBlowSledgehammer, new ItemInfo(ItemID.FierceBlowSledgehammer,
                iconPathDict[ItemID.FierceBlowSledgehammer],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Tools),
                new(2, 5)) },


            { ItemID.PhysicalBitcoin, new ItemInfo(ItemID.PhysicalBitcoin,
                iconPathDict[ItemID.PhysicalBitcoin],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Valuables),
                new(1, 1)) },

            { ItemID.CatFigurine, new ItemInfo(ItemID.CatFigurine,
                iconPathDict[ItemID.CatFigurine],
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Valuables),
                new(1, 3)) },


            { ItemID.BerkutBackpack, new BackpackItemInfo(ItemID.BerkutBackpack,
                iconPathDict[ItemID.BerkutBackpack],
                new(ItemMainCategory1.Gear, GearSubCategory2.Backpacks),
                new(4, 5),
                new RowColumn[] { new(5, 4) }) },


            { ItemID.AACPC, new TacticalRigItemInfo(ItemID.AACPC,
                iconPathDict[ItemID.AACPC],
                new(ItemMainCategory1.Gear, GearSubCategory2.TacticalRigs),
                new(4, 3),
                new RowColumn[] { new(2, 1), new(2, 1), new(2, 1), new(1, 1), new(1, 1), new(1, 1),
                    new(3, 1), new(3, 1), new(2, 2), new(2, 1), new(2, 1), }) },


            { ItemID.IntelligenceFolder, new ItemInfo(ItemID.IntelligenceFolder,
                iconPathDict[ItemID.IntelligenceFolder],
                new(ItemMainCategory1.InfoItems),
                new(2, 1)) },
        };
    }
}
