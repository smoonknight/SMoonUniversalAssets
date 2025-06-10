using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace SMoonUniversalAsset
{

    public class LanguageManager : SingletonWithDontDestroyOnLoad<LanguageManager>
    {
        public LanguageData commonLanguageData;
        public LanguageData conversationLanguageData;

        public LanguageType currentLanguage { get; private set; }

        public Dictionary<StringId, string> commonDictionary = new();
        public List<string> conversationDictionary = new();

        [SerializeField]
        [TextArea]
        private string indonesianEndOfDemoStoryMessage;
        [SerializeField]
        [TextArea]
        private string englishEndOfDemoStoryMessage;

        public string EndOfDemoStoryMessage { get; private set; }

        const string pattern = @"\[translateId:(\d+)\]";

        public void SetLanguages(LanguageType type)
        {
            currentLanguage = type;

            commonDictionary.Clear();
            string commonLanguage = GetTextAsset(type, commonLanguageData).text;
            List<string> commonList = JsonConvert.DeserializeObject<List<string>>(commonLanguage);

            System.Collections.IList stringIds = Enum.GetValues(typeof(StringId));
            for (int i = 0; i < stringIds.Count; i++)
            {
                commonDictionary.Add((StringId)stringIds[i], commonList[i]);
            }

            string conversationJson = GetTextAsset(type, conversationLanguageData).text;
            conversationDictionary = JsonConvert.DeserializeObject<List<string>>(conversationJson);

            TextMeshProUGUIOnTranslateBase[] TextMeshProUGUIOnTranslateBases = FindObjectsByType<TextMeshProUGUIOnTranslateBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (var TextMeshProUGUIOnTranslateBase in TextMeshProUGUIOnTranslateBases)
            {
                TextMeshProUGUIOnTranslateBase.Translate();
            }

            GameManager.Instance.InitAllAction();

            EndOfDemoStoryMessage = type switch
            {
                LanguageType.English => englishEndOfDemoStoryMessage,
                LanguageType.Indonesia => indonesianEndOfDemoStoryMessage,
                _ => throw new IndexOutOfRangeException(type + "not found!")
            };
        }

        public string GetDictionaryTextReplactor(StringId stringId, params object[] args)
        {
            string translateText = commonDictionary[stringId];
            return GetTextReplactor(translateText, args);
        }

        public string GetDictionaryTextReplactorOfConversation(int index, params object[] args)
        {
            return GetTextReplactor(conversationDictionary[index]);
        }

        public string GetTextReplactor(string text, params object[] args)
        {

            if (args != null && args.Length > 0 && Regex.IsMatch(text, @"\{\d+\}"))
            {
                text = Regex.Replace(text, @"\{(?!\d+\})([^\}]+)\}", @"{{${1}}}");
                text = string.Format(text, args);
            }

            return Regex.Replace(text, @"\{[^}]*\}", match => GetContextBySentence(match.Value));
        }
        string GetContextBySentence(string sentence)
        {
            return sentence switch
            {
                "{PlayerName}" => GameManager.Instance.GetPlayerName(),
                _ => throw new IndexOutOfRangeException($"{sentence} not found!"),
            };
        }

        TextAsset GetTextAsset(LanguageType language, LanguageData languageData)
        {
            return language switch
            {
                LanguageType.Indonesia => languageData.indonesianJson,
                LanguageType.English => languageData.englishJson,
                _ => throw new ArgumentOutOfRangeException(nameof(language), $"Unhandled language: {language}")
            };
        }


        public string GetTranslatedString(string originalString)
        {

            return Regex.Replace(originalString, pattern, match =>
            {
                int index = int.Parse(match.Groups[1].Value);

                if (index >= 0 && index < conversationDictionary.Count)
                {
                    return conversationDictionary[index];
                }
                else
                {
                    return "[Invalid translateId]";
                }
            });
        }

        // public StringId GetAddressableStringId(QuestStringId questStringId)
        // {
        //     return questStringId switch
        //     {
        //         QuestStringId.SlimeHunt => StringId.QuestStringIdSlimeHunt,
        //         QuestStringId.MimicMenace => StringId.QuestStringIdMimicMenace,
        //         QuestStringId.RabbyRampage => StringId.QuestStringIdRabbyRampage,
        //         QuestStringId.NPCConfrontation => StringId.QuestStringIdNPCConfrontation,
        //         QuestStringId.SnikeAmbush => StringId.QuestStringIdSnikeAmbush,
        //         _ => throw new IndexOutOfRangeException(questStringId + " not found!")
        //     };
        // }

        public StringId GetAddressableStringId(UpgradeType upgradeType) => upgradeType switch
        {
            UpgradeType.Character => StringId.UpgradeTypeCharacter,
            UpgradeType.MagicSword_Common => StringId.UpgradeTypeMagicSword_Common,
            UpgradeType.MagicSword_OnTarget => StringId.UpgradeTypeMagicSword_OnTarget,
            UpgradeType.MagicSword_Slashing => StringId.UpgradeTypeMagicSword_Slashing,
            _ => throw new NotImplementedException(),
        };

        public StringId GetAddressableStringId(UpgradeType upgradeType, UpgradeStatType upgradeStatType) => upgradeType switch
        {
            UpgradeType.Character => upgradeStatType switch
            {
                UpgradeStatType.size => StringId.CharacterSize,
                UpgradeStatType.attackInterval => StringId.CharacterAttackInterval,
                UpgradeStatType.speed => StringId.CharacterSpeed,
                UpgradeStatType.health => StringId.CharacterHealth,
                UpgradeStatType.damage => StringId.CharacterDamage,
                UpgradeStatType.jump => StringId.CharacterJump,
                UpgradeStatType.quantity => StringId.CharacterQuantity,
                _ => throw new NotImplementedException(),
            },

            UpgradeType.MagicSword_Common or
            UpgradeType.MagicSword_OnTarget or
            UpgradeType.MagicSword_Slashing => upgradeStatType switch
            {
                UpgradeStatType.size => StringId.MagicSwordSize,
                UpgradeStatType.attackInterval => StringId.MagicSwordAttackInterval,
                UpgradeStatType.speed => StringId.MagicSwordSpeed,
                UpgradeStatType.health => StringId.MagicSwordHealth,
                UpgradeStatType.damage => StringId.MagicSwordDamage,
                UpgradeStatType.jump => StringId.MagicSwordJump,
                UpgradeStatType.quantity => StringId.MagicSwordQuantity,
                _ => throw new NotImplementedException(),
            },

            _ => throw new NotImplementedException(),
        };

    }

    public static class LanguageHelper
    {
        public static string ToCommonLanguage(this StringId stringId) => LanguageManager.Instance.commonDictionary[stringId];
        public static string ToCommonLanguageOrDefault(this StringId stringId) => LanguageManager.Instance.commonDictionary[stringId] ?? stringId.ToString();
        public static string ToCommonLangaugeWithReplector(this StringId stringId, params object[] objects) => LanguageManager.Instance.GetDictionaryTextReplactor(stringId, objects);
    }

    public enum LanguageType
    {
        English,
        Indonesia,
    }
    [System.Serializable]
    public class LanguageData
    {
        public TextAsset englishJson;
        public TextAsset indonesianJson;
        public TextAsset japaneseJson;
    }
}