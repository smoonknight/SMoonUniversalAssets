using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class LanguageManager : SingletonWithDontDestroyOnLoad<LanguageManager>
{
    public LanguageLocateScriptableObject languageLocate;
    public LanguageType currentLanguage { get; private set; }

    List<CommonLanguageData> currentCommonLanguage;

    public Dictionary<StringId, string> commonDictionary = new();

    protected override void Awake()
    {
        base.Awake();
        SetLanguages();
    }

    public void SetLanguages() => SetLanguages(PlayerPrefsManager.Instance.GetLanguageOnPrefs());
    public void SetLanguages(LanguageType type)
    {
        PlayerPrefsManager.Instance.SetInt(IntPrefsEnum.Settings_Language_Locate_Index, (int)type);
        currentLanguage = type;

        LanguageLocateData selectedLocate = GetLanguageLocateData(type);

        commonDictionary.Clear();
        foreach (StringId stringId in Enum.GetValues(typeof(StringId)))
        {
            commonDictionary[stringId] = GetLanguageData(stringId);
        }

    }

    LanguageLocateData GetLanguageLocateData(LanguageType language)
    {
        return language switch
        {
            LanguageType.Indonesia => languageLocate.indonesiaLanguageLocateData,
            LanguageType.English => languageLocate.englishLanguageLocateData,
            _ => throw new ArgumentOutOfRangeException(nameof(language), $"Unhandled language: {language}")
        };
    }


    // public string GetAddressTitleLanguage(LevelGameMode levelGameMode)
    // {
    //     return levelGameMode switch
    //     {
    //         LevelGameMode.ELIMINATE => commonDictionary[StringId.GAMEMODE_TITLE_ELIMINATE],
    //         LevelGameMode.PROTECT_THE_RESOURCE => commonDictionary[StringId.GAMEMODE_TITLE_PROTECT_THE_SOURCE],
    //         LevelGameMode.SCORE_CHALLENGE => commonDictionary[StringId.GAMEMODE_TITLE_SCORE_CHALLANGE],
    //         LevelGameMode.TIME_CHALLENGE => commonDictionary[StringId.GAMEMODE_TITLE_TIME_CHALLANGE],
    //         LevelGameMode.BOSS_FIGHT => commonDictionary[StringId.GAMEMODE_TITLE_BOSS_FIGHT],
    //         _ => throw new ArgumentOutOfRangeException(nameof(levelGameMode), $"Unhandled levelGameMode: {levelGameMode}")
    //     };
    // }

    string GetLanguageData(StringId stringId) => currentCommonLanguage.Find(languageDictionary => languageDictionary.stringId == stringId).text;
}

public enum LanguageType
{
    Indonesia,
    English
}