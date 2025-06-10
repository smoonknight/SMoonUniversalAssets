using System;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public class SettingManager : SingletonWithDontDestroyOnLoad<SettingManager>
    {
        [SerializeField]
        private SettingView settingView;
        [SerializeField]
        private AudioName sampleSFXAudioName;

        PlayerSetting playerSetting;

        protected override void OnAwake()
        {
            base.OnAwake();

            playerSetting = SaveSystem.LoadPlayerSetting();
        }
        private void Start()
        {
            Initialize();
            if (!playerSetting.hasFirstLaunchSetting)
            {
                Raise(false);
                playerSetting.hasFirstLaunchSetting = true;
            }
        }

        public void Initialize()
        {
            AudioExtendedManager.Instance.SetAudioMixerSFXVolume(playerSetting.soundSFXPercentage);
            AudioExtendedManager.Instance.SetAudioMixerBGMVolume(playerSetting.soundBGMPercentage);
            AudioExtendedManager.Instance.SetAudioMixerVoiceVolume(playerSetting.soundVoicePercentage);
            LanguageManager.Instance.SetLanguages(playerSetting.languageType);
            QualitySettings.SetQualityLevel(playerSetting.qualitySettingsIndex);
            Application.targetFrameRate = playerSetting.targetFrameRate;
        }

        public LanguageType LanguageType => playerSetting.languageType;

        public void Raise(bool showMainMenuButton)
        {
            SetMainMenuButtonCondition(showMainMenuButton);
            settingView.gameObject.SetActive(true);
            playerSetting = SaveSystem.LoadPlayerSetting();
            settingView.Initialize(playerSetting);
        }

        public void SetMainMenuButtonCondition(bool condition)
        {
            settingView.SetMainMenuButtonCondition(condition);
        }

        public void Close()
        {
            SavePlayerSettingAndSet(playerSetting);
            settingView.gameObject.SetActive(false);
        }

        public void SavePlayerSettingAndSet(PlayerSetting playerSetting)
        {
            SaveSystem.SavePlayerSetting(playerSetting);
            Initialize();
        }

        public void Reset()
        {
            playerSetting = GetDefaultPlayerSetting();
            SavePlayerSettingAndSet(playerSetting);
            settingView.Initialize(playerSetting);
        }

        public PlayerSetting GetDefaultPlayerSetting() => new()
        {
            soundSFXPercentage = 1,
            soundBGMPercentage = 1,
            soundVoicePercentage = 1,
            languageType = LanguageType.Indonesia,
            qualitySettingsIndex = 2,
            targetFrameRate = 60,
            hasFirstLaunchSetting = false,
        };

        public LanguageType GetLanguageTypeBySystemLangauge()
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.Indonesian => LanguageType.Indonesia,
                SystemLanguage.English => LanguageType.English,
                _ => LanguageType.English
            };
        }

        public void ChangeAudioSFXValue(float value)
        {
            playerSetting.soundSFXPercentage = value;
            AudioExtendedManager.Instance.SetAudioMixerSFXVolume(value);
            AudioExtendedManager.Instance.Play(sampleSFXAudioName);
        }

        public void ChangeAudioBGMValue(float value)
        {
            playerSetting.soundBGMPercentage = value;
            AudioExtendedManager.Instance.SetAudioMixerBGMVolume(value);
        }

        public void ChangeLanguage(LanguageType languageType)
        {
            playerSetting.languageType = languageType;
            LanguageManager.Instance.SetLanguages(languageType);
        }

        public void ChangeQuality(int index)
        {
            playerSetting.qualitySettingsIndex = index;
            QualitySettings.SetQualityLevel(index);
        }

        public void ChangeTargetFPS(int targetFrameRate)
        {
            playerSetting.targetFrameRate = targetFrameRate;
            Application.targetFrameRate = targetFrameRate;
        }
    }
}