using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageLocateScriptObject", menuName = "")]
public class LanguageLocateScriptableObject : ScriptableObject
{
    public LanguageLocateData indonesiaLanguageLocateData;
    public LanguageLocateData englishLanguageLocateData;
    public LanguageLocateData japaneseLanguageLocateData;
    public List<SpecialNameLangaugeData> specialNameLangaugeDatas;
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
    [TextArea]
    public string text;
}

[System.Serializable]
public struct SpecialNameLangaugeData
{
    public int index;
    public string text;
}