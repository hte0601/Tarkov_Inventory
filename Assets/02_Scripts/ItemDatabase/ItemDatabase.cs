using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    public static Dictionary<ItemID, ItemInfo> info;
    public static Dictionary<ItemID, string> iconPath;

    static ItemDatabase()
    {
        info = new()
        {
            { ItemID.GraphicsCard, new ItemInfo(new(2, 1),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics)) },

            { ItemID.Tetriz, new ItemInfo(new(1, 2),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics)) },

            { ItemID.PhasedArrayElement, new ItemInfo(new(2, 2),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics)) },

            { ItemID.MilitaryPowerFilter, new ItemInfo(new(1, 1),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Electronics)) },


            { ItemID.MilitaryBattery, new ItemInfo(new(4, 2),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.EnergyElements)) },


            { ItemID.LEDX, new ItemInfo(new(1, 1),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.MedicalSupplies)) },


            { ItemID.FierceBlowSledgehammer, new ItemInfo(new(2, 5),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Tools)) },


            { ItemID.PhysicalBitcoin, new ItemInfo(new(1, 1),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Valuables)) },

            { ItemID.CatFigurine, new ItemInfo(new(1, 3),
                new(ItemMainCategory1.BarterItems, BarterItemSubCategory2.Valuables)) },


            { ItemID.BerkutBackpack, new BackpackItemInfo(new(4, 5),
                new(ItemMainCategory1.Gear, GearSubCategory2.Backpacks),
                new RowColumn[] { new(5, 4) }) },


            { ItemID.AACPC, new TacticalRigItemInfo(new(4, 3),
                new(ItemMainCategory1.Gear, GearSubCategory2.TacticalRigs),
                new RowColumn[] { new(2, 1), new(2, 1), new(2, 1), new(1, 1), new(1, 1), new(1, 1),
                new(3, 1), new(3, 1), new(2, 2), new(2, 1), new(2, 1), }) },


            { ItemID.IntelligenceFolder, new ItemInfo(new(2, 1),
                new(ItemMainCategory1.InfoItems)) },
        };

        iconPath = new()
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
    }
}
