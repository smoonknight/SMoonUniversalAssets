using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : SingletonWithDontDestroyOnLoad<PlayerPrefsManager>
{
    const string privateKey = "987bcab01b929eb2c07877b224215c92";
    protected override void Awake()
    {
        base.Awake();
        SetDefaultFloatPrefsDictionary();
        SetDefaultIntPrefsDictionary();

        if (!PlayerPrefs.HasKey(privateKey))
        {
            DefaultGameplay();
            DefaultSettings();
            PlayerPrefs.SetInt(privateKey, 0);
        }
    }

    Dictionary<FloatPrefsEnum, float> defaultFloatPrefsDictionary = new();
    Dictionary<IntPrefsEnum, int> defaultIntPrefsDictionary = new();

    public void SetDefaultFloatPrefsDictionary()
    {
        defaultFloatPrefsDictionary.Add(FloatPrefsEnum.Settings_Audio_BGM_Volume, 1);
        defaultFloatPrefsDictionary.Add(FloatPrefsEnum.Settings_Audio_SFX_Volume, 1);
        defaultFloatPrefsDictionary.Add(FloatPrefsEnum.Settings_Audio_Voice_Volume, 1);
        defaultFloatPrefsDictionary.Add(FloatPrefsEnum.Settings_Control_Camera_AimSensitivity, 1);
        defaultFloatPrefsDictionary.Add(FloatPrefsEnum.Settings_Control_Camera_SightSensitivity, 1);
    }

    public void SetDefaultIntPrefsDictionary()
    {
        defaultIntPrefsDictionary.Add(IntPrefsEnum.Settings_Graphic_Quality_Index, 1);
        defaultIntPrefsDictionary.Add(IntPrefsEnum.Settings_Language_Locate_Index, 0);
        defaultIntPrefsDictionary.Add(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked, 0);
        defaultIntPrefsDictionary.Add(IntPrefsEnum.Gameplay_Currency_Experience, 0);
    }

    public SettingPrefsData GetSettingPrefsData()
    {
        SettingPrefsData settingData = new()
        {
            audioBGMVolume = GetFloat(FloatPrefsEnum.Settings_Audio_BGM_Volume),
            audioSFXVolume = GetFloat(FloatPrefsEnum.Settings_Audio_SFX_Volume),
            audioVoiceVolume = GetFloat(FloatPrefsEnum.Settings_Audio_Voice_Volume),
            settingGraphicType = GetSettingGraphicTypeOnPrefs(),
            languageType = GetLanguageOnPrefs(),
            aimCameraSensitivity = GetFloat(FloatPrefsEnum.Settings_Control_Camera_AimSensitivity),
            sightCameraSensitivity = GetFloat(FloatPrefsEnum.Settings_Control_Camera_SightSensitivity),
        };
        return settingData;
    }

    public void SetSettingPrefsData(SettingPrefsData settingData)
    {
        SetFloat(FloatPrefsEnum.Settings_Audio_BGM_Volume, settingData.audioBGMVolume);
        SetFloat(FloatPrefsEnum.Settings_Audio_SFX_Volume, settingData.audioSFXVolume);
        SetFloat(FloatPrefsEnum.Settings_Audio_SFX_Volume, settingData.audioVoiceVolume);
        SetInt(IntPrefsEnum.Settings_Graphic_Quality_Index, (int)settingData.settingGraphicType);
        SetInt(IntPrefsEnum.Settings_Language_Locate_Index, (int)settingData.languageType);
        SetFloat(FloatPrefsEnum.Settings_Control_Camera_SightSensitivity, settingData.sightCameraSensitivity);

        // LanguageManager.Instance.SetLanguages(settingData.languageType);

    }

    [ContextMenu("Debug")]
    void DebugMode() => Debug.Log(GetInt(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked));
    [ContextMenu("Debug Extension")]
    void DebugModeExtension()
    {
        SetInt(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked, 19);
        DebugMode();
    }

    public float GetFloat(FloatPrefsEnum floatPrefsEnum) => PlayerPrefs.GetFloat(floatPrefsEnum.ToString());
    public void SetFloat(FloatPrefsEnum floatPrefsEnum, float value) => PlayerPrefs.SetFloat(floatPrefsEnum.ToString(), value);
    public int GetInt(IntPrefsEnum intPrefsEnum) => PlayerPrefs.GetInt(intPrefsEnum.ToString());
    public void SetInt(IntPrefsEnum intPrefsEnum, int value) => PlayerPrefs.SetInt(intPrefsEnum.ToString(), value);

    public void AddInt(IntPrefsEnum intPrefsEnum, int value)
    {
        int neurocoin = GetInt(intPrefsEnum);
        neurocoin += value;
        SetInt(intPrefsEnum, neurocoin);
    }

    public void AddInt(IntPrefsEnum intPrefsEnum, int value, out int getInt)
    {
        AddInt(intPrefsEnum, value);
        getInt = GetInt(intPrefsEnum);
    }

    public void SetLevelProgressionIndex(int target)
    {
        if (GetInt(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked) <= target)
        {
            SetInt(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked, target);
        }
    }
    public void SetDefault(FloatPrefsEnum floatPrefsEnum) => PlayerPrefs.SetFloat(floatPrefsEnum.ToString(), defaultFloatPrefsDictionary[floatPrefsEnum]);
    public void SetDefault(IntPrefsEnum intPrefsEnum) => PlayerPrefs.SetInt(intPrefsEnum.ToString(), defaultIntPrefsDictionary[intPrefsEnum]);

    public LanguageType GetLanguageOnPrefs()
    {
        return (float)GetInt(IntPrefsEnum.Settings_Language_Locate_Index) switch
        {
            0 => LanguageType.Indonesia,
            1 => LanguageType.English,
            _ => LanguageType.English,
        };
    }

    public void SetDefaultLanguage()
    {
        SystemLanguage defaultLanguage = Application.systemLanguage;
        SetInt(IntPrefsEnum.Settings_Language_Locate_Index, defaultLanguage == SystemLanguage.Indonesian ? 0 : 1);
    }

    public SettingGraphicType GetSettingGraphicTypeOnPrefs()
    {
        return GetInt(IntPrefsEnum.Settings_Graphic_Quality_Index) switch
        {
            0 => SettingGraphicType.Low,
            1 => SettingGraphicType.Med,
            2 => SettingGraphicType.High,
            _ => SettingGraphicType.Med,
        };
    }

    public void DefaultGameplay()
    {
        SetDefault(IntPrefsEnum.Gameplay_Progression_LevelIndexUnlocked);
        SetDefault(IntPrefsEnum.Gameplay_Currency_Experience);
    }

    public void DefaultSettings()
    {
        SetDefault(FloatPrefsEnum.Settings_Audio_BGM_Volume);
        SetDefault(FloatPrefsEnum.Settings_Audio_SFX_Volume);
        SetDefault(FloatPrefsEnum.Settings_Audio_Voice_Volume);
        SetDefault(FloatPrefsEnum.Settings_Control_Camera_SightSensitivity);
        SetDefault(FloatPrefsEnum.Settings_Control_Camera_AimSensitivity);
        SetDefault(IntPrefsEnum.Settings_Graphic_Quality_Index);
        SetDefault(IntPrefsEnum.Settings_Language_Locate_Index);
        SetDefaultLanguage();
    }
}

public enum FloatPrefsEnum
{
    Settings_Audio_BGM_Volume,
    Settings_Audio_SFX_Volume,
    Settings_Audio_Voice_Volume,
    Settings_Control_Camera_SightSensitivity,
    Settings_Control_Camera_AimSensitivity,
}

public enum IntPrefsEnum
{
    Settings_Graphic_Quality_Index,
    Settings_Language_Locate_Index,
    Gameplay_Progression_LevelIndexUnlocked,
    Gameplay_Currency_Experience,
    Gameplay_Weapon_ObtainedCondition,
    Gameplay_Weapon_Level,
}

public enum StringPrefsEnum
{
    Gameplay_Progression_GoalAchivmentOnLevelIndex,
}

public class SettingPrefsData
{
    public float audioBGMVolume;
    public float audioSFXVolume;
    public float audioVoiceVolume;
    public SettingGraphicType settingGraphicType;
    public LanguageType languageType;
    public float sightCameraSensitivity;
    public float aimCameraSensitivity;
}

public enum SettingGraphicType
{
    Low,
    Med,
    High
}