using System.Collections.Generic;
using UnityEngine;

public class ItemProcessor
{
    private static readonly Dictionary<Item, string> itemNames = new Dictionary<Item, string>()
    {
        { Item.Coin, "Coin" },
        { Item.RawMeat, "Raw Meat" },
        { Item.RabbyHorn, "Rabby Horn" },
        { Item.Skin, "Skin" },
        { Item.Fang, "Fang" },
        { Item.BlankChest, "Blank Chest" },
        { Item.MimicTongue, "Mimic Tongue" },
        { Item.SnikiBlood, "Sniki Blood" },
        { Item.SnikiFang, "Sniki Fang" },
        { Item.Cloth, "Cloth" },
        { Item.Gelatin, "Gelatin" },
        { Item.Book, "Book" },
        { Item.Yakisoba, "Yakisoba" },
        { Item.AdultBook, "Adult Book" },
        { Item.MelonPan, "Melon Pan" },
        { Item.Candy, "Candy" },
        { Item.Sugar, "Sugar" },
        { Item.BrokenCrystal, "Broken Crystal" },
        { Item.Markup, "Markup" }
    };

    private static readonly Dictionary<RarityDrop, float> rarityDropRates = new Dictionary<RarityDrop, float>() {
        {RarityDrop.Guaranteed, 1f},
        {RarityDrop.Common, 0.6f},
        {RarityDrop.Uncommon, 0.4f},
        {RarityDrop.Rare, 0.2f},
        {RarityDrop.Epic, 0.1f}
    };

    public static float GetRarityRate(RarityDrop rarityDrop)
    {
        if (rarityDropRates.ContainsKey(rarityDrop))
        {
            return rarityDropRates[rarityDrop];
        }
        else
        {
            Debug.Log($"{rarityDrop} belum disetting pada dictionary");
            return 0.6f;
        }
    }

    public static string GetItemNameWithSpaces(Item item)
    {
        if (itemNames.ContainsKey(item))
        {
            return itemNames[item];
        }
        else
        {
            return "Unknown Item";
        }
    }
}

public enum Item
{
    Coin,
    RawMeat,
    RabbyHorn,
    Skin,
    Fang,
    BlankChest,
    MimicTongue,
    SnikiBlood,
    SnikiFang,
    Cloth,
    Gelatin,
    Book,
    Yakisoba,
    AdultBook,
    MelonPan,
    Candy,
    Sugar,
    BrokenCrystal,
    Markup
}


public enum RarityDrop
{
    Guaranteed,
    Common,
    Uncommon,
    Rare,
    Epic,
}