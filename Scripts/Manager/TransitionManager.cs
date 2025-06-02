using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TransitionManager : SingletonWithDontDestroyOnLoad<TransitionManager>
{
    public TransitionData[] transitionDatas;
    public RectTransform transitionCanvas;

    TransitionType lastTransitionType;
    SceneEnum lastSceneManagerEnum;

    public bool isOnProgressTransitionScene { get; private set; }

    private void Start()
    {
        SceneHelper.CheckCurrentSceneRequire();
    }

    public void SetTransition(TransitionType type, UnityAction midTransitionCallback, UnityAction endTransitionCallback, float delayDuration = 0)
    {
        if (type == TransitionType.None)
        {
            midTransitionCallback?.Invoke();
            endTransitionCallback?.Invoke();
            return;
        }
        TransitionData transitionData = Array.Find(transitionDatas, transitionData => transitionData.transitionType == type);
        CanvasGroup transition = Instantiate(transitionData.transitionImage, transitionCanvas);

        transition.alpha = 0;

        LeanTween.alphaCanvas(transition, 1, 0.5f).setEaseInExpo().setOnComplete(() =>
        {
            midTransitionCallback?.Invoke();

            var sequence = LeanTween.sequence();

            sequence.append(delayDuration);
            sequence.append(() =>
            {
                LeanTween.alphaCanvas(transition, 0, 0.5f).setEaseInExpo().setOnComplete(() =>
                {
                    endTransitionCallback?.Invoke();
                    Destroy(transition.gameObject);
                });
            });
        });
    }

    public void SetTransitionOnSceneManagerPrevious() => SetTransitionOnSceneManager(lastTransitionType, lastSceneManagerEnum);
    public void SetTransitionOnSceneManager(TransitionType type, SceneEnum sceneManagerEnum) => SetTransitionOnSceneManager(type, sceneManagerEnum, () => { }, () => { });
    public void SetTransitionOnSceneManager(TransitionType type, SceneEnum sceneEnum, UnityAction midTransitionCallback, UnityAction endTransitionCallback)
    {
        lastSceneManagerEnum = sceneEnum;
        isOnProgressTransitionScene = true;

        lastTransitionType = type;
        TransitionData transitionData = Array.Find(transitionDatas, transitionData => transitionData.transitionType == type);
        CanvasGroup transition = Instantiate(transitionData.transitionImage, transitionCanvas);

        midTransitionCallback?.Invoke();

        transition.alpha = 0;

        LeanTween.alphaCanvas(transition, 1, 1).setEaseInExpo().setOnComplete(() =>
        {
            endTransitionCallback += () => isOnProgressTransitionScene = false;
            StartTransition(transition, sceneEnum, endTransitionCallback);
        }).setIgnoreTimeScale(true);
    }

    async void StartTransition(CanvasGroup transition, SceneEnum sceneEnum, UnityAction endTransitionCallback)
    {
        var sceneName = SceneHelper.GetSceneBySceneEnum(sceneEnum);
        var operationAsync = SceneManager.LoadSceneAsync(sceneName);

        SceneHelper.CheckSceneRequire(sceneEnum);

        Screen.sleepTimeout = lastSceneManagerEnum != SceneEnum.MAINMENU ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;

        operationAsync.completed += (async) =>
        {
            LeanTween.alphaCanvas(transition, 0, 1).setEaseInExpo();
            endTransitionCallback?.Invoke();
            Destroy(transition.gameObject);
        };
        await UniTask.WaitUntil(() => operationAsync.isDone);
    }

    [System.Serializable]
    public struct TransitionData
    {
        public TransitionType transitionType;
        public CanvasGroup transitionImage;
    }
}

[System.Serializable]
public class SceneManagerData
{
    public SceneManagerData(string sceneName, bool isScreenNeverSleep)
    {
        this.sceneName = sceneName;
        this.isScreenNeverSleep = isScreenNeverSleep;
    }
    public string sceneName;
    public bool isScreenNeverSleep;
}

public enum TransitionType
{
    None,
    Black,
    White,
    Loading,
    OnRoad
}