using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SMoonUniversalAsset
{
    public class SettingView : ViewBase
    {
        [SerializeField]
        private Slider audioSFXSlider;
        [SerializeField]
        private Slider audioBGMSlider;
        [SerializeField]
        private RectTransform languageSelectorPanel;
        [SerializeField]
        private RectTransform qualitySelectorPanel;
        [SerializeField]
        private RectTransform fpsTargetSelectorPanel;
        [SerializeField]
        private ButtonView buttonViewTemplate;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private Button mainMenuButton;
        [SerializeField]
        private Button resetButton;

        private List<ButtonView> langaugeButtonViews = new();
        private List<ButtonView> qualityButtonViews = new();
        private List<ButtonView> fpsButtonViews = new();

        private readonly List<int> targetFrameRates = new() {
        30, 60, -1
    };

        private void OnEnable()
        {
            audioSFXSlider.onValueChanged.AddListener((value) => SettingManager.Instance.ChangeAudioSFXValue(value));
            audioBGMSlider.onValueChanged.AddListener((value) => SettingManager.Instance.ChangeAudioBGMValue(value));

            closeButton.onClick.AddListener(() => SettingManager.Instance.Close());
            mainMenuButton.onClick.AddListener(() =>
            {
                SettingManager.Instance.Close();
                TransitionManager.Instance.SetTransitionOnSceneManager(TransitionType.Black, SceneEnum.MAINMENU);
            });
            resetButton.onClick.AddListener(() => SettingManager.Instance.Reset());
        }

        private void OnDisable()
        {
            audioSFXSlider.onValueChanged.RemoveListener((value) => SettingManager.Instance.ChangeAudioSFXValue(value));
            audioBGMSlider.onValueChanged.RemoveListener((value) => SettingManager.Instance.ChangeAudioBGMValue(value));

            closeButton.onClick.RemoveListener(() => SettingManager.Instance.Close());
            mainMenuButton.onClick.RemoveListener(() =>
            {
                SettingManager.Instance.Close();
                TransitionManager.Instance.SetTransitionOnSceneManager(TransitionType.Black, SceneEnum.MAINMENU);
            });
            resetButton.onClick.RemoveListener(() => SettingManager.Instance.Reset());
        }

        public void SetMainMenuButtonCondition(bool condition)
        {
            mainMenuButton.gameObject.SetActive(condition);
        }

        public void Initialize(PlayerSetting playerSetting)
        {
            audioSFXSlider.value = playerSetting.soundSFXPercentage;
            audioBGMSlider.value = playerSetting.soundBGMPercentage;

            languageSelectorPanel.DestroyChilds();
            qualitySelectorPanel.DestroyChilds();
            fpsTargetSelectorPanel.DestroyChilds();

            langaugeButtonViews.Clear();
            qualityButtonViews.Clear();
            fpsButtonViews.Clear();

            foreach (LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
            {
                var langaugeButtonView = Instantiate(buttonViewTemplate, languageSelectorPanel);
                langaugeButtonView.gameObject.SetActive(true);
                langaugeButtonView.Initialize(languageType.ToString(), () =>
                {
                    SettingManager.Instance.ChangeLanguage(languageType);
                    SwitchButton(langaugeButtonViews, langaugeButtonView);
                });

                langaugeButtonViews.Add(langaugeButtonView);
            }

            var currentLangaugeButtonView = langaugeButtonViews.FirstOrDefault(b => b.Text == playerSetting.languageType.ToString());
            if (currentLangaugeButtonView != null)
            {
                SwitchButton(langaugeButtonViews, currentLangaugeButtonView);
            }

            var qualitySettingsNames = QualitySettings.names;
            for (int i = 0; i < qualitySettingsNames.Count(); i++)
            {
                var qualityButtonView = Instantiate(buttonViewTemplate, qualitySelectorPanel);
                var localIndex = i;
                qualityButtonView.gameObject.SetActive(true);
                qualityButtonView.Initialize(qualitySettingsNames[i], () =>
                {
                    SettingManager.Instance.ChangeQuality(localIndex);
                    SwitchButton(qualityButtonViews, qualityButtonView);
                });

                qualityButtonViews.Add(qualityButtonView);
            }

            if (playerSetting.qualitySettingsIndex < qualitySettingsNames.Length)
            {
                var currentqualityButtonView = qualityButtonViews.FirstOrDefault(b => qualitySettingsNames[playerSetting.qualitySettingsIndex] == b.Text);
                if (currentqualityButtonView != null)
                {
                    SwitchButton(qualityButtonViews, currentqualityButtonView);
                }
            }

            foreach (var targetFrameRate in targetFrameRates)
            {
                var localTargetFrameRate = targetFrameRate;
                var fpsButtonView = Instantiate(buttonViewTemplate, fpsTargetSelectorPanel);
                fpsButtonView.gameObject.SetActive(true);
                fpsButtonView.Initialize(localTargetFrameRate == -1 ? "Unlimited" : targetFrameRate.ToString(), () =>
                {
                    SettingManager.Instance.ChangeTargetFPS(localTargetFrameRate);
                    SwitchButton(fpsButtonViews, fpsButtonView);
                });

                fpsButtonViews.Add(fpsButtonView);
            }

            var currentfpsButtonView = fpsButtonViews.FirstOrDefault(b => b.Text == (playerSetting.targetFrameRate == -1 ? "Unlimited" : playerSetting.targetFrameRate.ToString()));
            if (currentfpsButtonView != null)
            {
                SwitchButton(fpsButtonViews, currentfpsButtonView);
            }
        }

        void SwitchButton(List<ButtonView> buttonViews, ButtonView buttonView)
        {
            buttonViews.ForEach(action => action.ChangeButtonColor(Color.gray));
            buttonView.ChangeButtonColor(Color.white);
        }
    }
}