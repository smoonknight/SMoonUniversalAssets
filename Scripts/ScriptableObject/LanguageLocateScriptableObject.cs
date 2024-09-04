using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageLocateScriptObject", menuName = "")]
public class LanguageLocateScriptableObject : ScriptableObject
{
    public LanguageLocateData indonesiaLanguageLocateData;
    public LanguageLocateData englishLanguageLocateData;
}

[System.Serializable]
public struct LanguageLocateData
{
    public List<CommonLanguageData> commonLanguageDatas;
}

[System.Serializable]
public struct CommonLanguageData
{
    public StringId stringId;
    public string text;
}


public enum StringId
{
    LOCKED,
    FAILED_VALIDATE_MUST_CARRY_AT_LEAST_ONE,
    FAILED_VALIDATE_LEVELDATA_NOT_LOADED,
    SUCCESS_VALIDATE_LEVELDATA_NOT_LOADED,
    GAMEMODE_TITLE_ELIMINATE,
    GAMEMODE_DESCRIPTION_ELIMINATE,
    GAMEMODE_TITLE_PROTECT_THE_SOURCE,
    GAMEMODE_DESCRIPTION_PROTECT_THE_SOURCE,
    GAMEMODE_TITLE_SCORE_CHALLANGE,
    GAMEMODE_DESCRIPTION_SCORE_CHALLANGE,
    GAMEMODE_TITLE_TIME_CHALLANGE,
    GAMEMODE_DESCRIPTION_TIME_CHALLANGE,
    GAMEMODE_TITLE_BOSS_FIGHT,
    GAMEMODE_DESCRIPTION_BOSS_FIGHT,
    WILL_BE_UNLOCKED_AFTER_STAGEINDEX,
    SHOP_WEAPON_SLINGSHOT_DESCRIPTION,
    SHOP_WEAPON_CROSSBOW_DESCRIPTION,
    SHOP_WEAPON_HANDGUN_DESCRIPTION,
    SHOP_WEAPON_SNIPER_DESCRIPTION,
    SHOP_BUTTON_BUY,
    SHOP_BUTTON_UPGRADE,
    FAILED_VALIDATE_ITEMHASOBTAINED,
    FAILED_VALIDATE_ITEMNOTOBTAINED,
    FAILED_VALIDATE_NOTENOUGHEXPERIENCE,
    SUCCESS_VALIDATE_ITEMPURCHASECOMPLETE,
    TIPS_FAILED_STAGE
}