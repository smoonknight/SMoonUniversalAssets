using System.Collections.Generic;
using UnityEngine;

public static class ItemProcessor
{
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

    public static ItemType GetAddressableItemType(FishItemType fishItemType)
    {
        return fishItemType switch
        {
            FishItemType.Fishnishim => ItemType.Fishnishim,
            FishItemType.StripedPuffinno => ItemType.StripedPuffinno,
            FishItemType.Swordfish => ItemType.Swordfish,
            FishItemType.Dugong => ItemType.Dugong,
            FishItemType.Catfish => ItemType.Catfish,
            FishItemType.Mohawksquid => ItemType.Mohawksquid,
            FishItemType.Cherrypuff => ItemType.Cherrypuff,
            FishItemType.Untitled => ItemType.Untitled,
            FishItemType.AlphaWhale => ItemType.AlphaWhale,
            _ => throw new System.NotImplementedException(),
        };
    }

    public static FishItemType GetFishItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Fishnishim => FishItemType.Fishnishim,
            ItemType.StripedPuffinno => FishItemType.StripedPuffinno,
            ItemType.Swordfish => FishItemType.Swordfish,
            ItemType.Dugong => FishItemType.Dugong,
            ItemType.Catfish => FishItemType.Catfish,
            ItemType.Mohawksquid => FishItemType.Mohawksquid,
            ItemType.Cherrypuff => FishItemType.Cherrypuff,
            ItemType.Untitled => FishItemType.Untitled,
            ItemType.AlphaWhale => FishItemType.AlphaWhale,
            _ => throw new System.NotImplementedException(),
        };
    }
}

public enum ItemType
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
    Markup,
    ZStamina,
    Medicine,
    TeaPack,
    CoffeeCup,
    IceCream,
    Condom,
    FirstAid,
    BandageStrip,
    SuspiciousMedicine,
    RoastedChicken,
    NasiGoreng,
    RiceSalt,
    Omelet,
    Steak,
    Salad,
    SweetCandy,
    TeddyBear,
    SchoolStationery,
    StrawberryCake,
    Fishnishim,
    StripedPuffinno,
    Swordfish,
    Dugong,
    Catfish,
    Mohawksquid,
    Cherrypuff,
    Untitled,
    AlphaWhale,
    Balloon,
    Boots,
    TrashCan,
    BrokenNet,
    Sandal,
    TrashChest,
    Collar,
    FilledCan,
    GreenAlgae,
    Pants,
    Pearl,
    Ring,
    TreasureChest,
    FishingRod
}

public enum FishItemType
{
    Fishnishim,
    StripedPuffinno,
    Swordfish,
    Dugong,
    Catfish,
    Mohawksquid,
    Cherrypuff,
    Untitled,
    AlphaWhale,
}

public enum ItemCategoryType
{
    NonUseable,
    Useable,
    Key,
    Fish,
    Trash
}


public enum RarityDrop
{
    Guaranteed,
    Common,
    Uncommon,
    Rare,
    Epic,
}