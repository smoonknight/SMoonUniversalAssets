using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class AudioExtendedManager : SingletonWithDontDestroyOnLoad<AudioExtendedManager>
{
    public AudioMixerGroupData audioMixerGroupData;
    public AnimationCurve customRollOffCurve;
    [SerializeField]
    private AudioName clickAudioName;
    public Sound[] sounds;

    Dictionary<AudioName, Sound> soundDictionary = new();

    MusicName? currentMusicName = null;
    AudioName? currentMusicAudioName = null;

    private DefaultInputActions inputActions;

    float VolumeMixer(float value) => Mathf.Lerp(-40f, 0f, value);
    float FocusMixer(float value) => Mathf.Lerp(-50f, 0f, value);
    float LowpassMixer(float value) => Mathf.Lerp(10, 22000, value);
    public void SetAudioMixerBGMVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.BGM, volume);
    public void SetAudioMixerSFXVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.SFX, volume);
    public void SetAudioMixerVoiceVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.Voice, volume);

    public void SetAudioMixerBGMLowpass(float value, bool isSmoothly = true)
    {
        if (isSmoothly)
        {
            SetAudioMixerBGMLowpassSmoothly(value);
        }
        else
        {
            SetAudioMixerParam(audioMixerGroupData.BGM, LowpassMixer(value), "Lowpass BGM");
        }
    }

    public void SetAudioMixerBGMLowpassSmoothly(float value)
    {
        if (leanTweenLowpassId != 0) LeanTween.cancel(leanTweenLowpassId);
        GetAudioMixerParam(audioMixerGroupData.BGM, "Lowpass BGM", out float currentValue);
        leanTweenLowpassId = LeanTween.value(currentValue, LowpassMixer(value), 0.5f).setOnUpdate((value) =>
        {
            SetAudioMixerParam(audioMixerGroupData.BGM, value, "Lowpass BGM");
        }).setIgnoreTimeScale(true).id;
    }

    public void SetAudioMixerBGMFocus(float value) => SetAudioMixerParam(audioMixerGroupData.BGM, FocusMixer(value), "FocusBGM");
    public void SetAudioMixerBGMSmoothFocus(float from, float to, bool isUnscaleTime)
    {
        LeanTween.value(gameObject, (float v) => SetAudioMixerBGMFocus(v), from, to, 1f)
                 .setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(isUnscaleTime);
    }
    public void SetAudioMixerBGMSmoothToFocus(bool isUnscaleTime = true) => SetAudioMixerBGMSmoothFocus(0, 1, isUnscaleTime);
    public void SetAudioMixerBGMSmoothToUnfocus(bool isUnscaleTime = true) => SetAudioMixerBGMSmoothFocus(1, 0, isUnscaleTime);

    public void SetAudioMixerVolume(AudioMixerGroup audioMixerGroup, float volume) => SetAudioMixerParam(audioMixerGroup, VolumeMixer(volume), audioMixerGroup.name);
    public void SetAudioMixerParam(AudioMixerGroup audioMixerGroup, float volume, string param) => audioMixerGroup.audioMixer.SetFloat(param, volume);
    public void GetAudioMixerParam(AudioMixerGroup audioMixerGroup, string param, out float value) => audioMixerGroup.audioMixer.GetFloat(param, out value);

    int leanTweenLowpassId = 0;

    protected override void OnAwake()
    {
        base.OnAwake();

        inputActions = new();
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = 1;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = GetAudioMixerGroup(sound.audioType);
            sound.source.playOnAwake = false;
        }

        SetSoundDictionary();
    }

    Action<InputAction.CallbackContext> ClickCallbackContext => (ctx) => Play(clickAudioName);
    public AudioMixerGroup GetAudioMixerGroup(AudioType audioType)
    {
        return audioType switch
        {
            AudioType.BGM => audioMixerGroupData.BGM,
            AudioType.SFX => audioMixerGroupData.SFX,
            AudioType.VOICE => audioMixerGroupData.Voice,
            _ => audioMixerGroupData.BGM,
        };
    }

    public void PlayClickSound() => Play(clickAudioName);

    public void SetSoundDictionary()
    {
        foreach (AudioName audioName in Enum.GetValues(typeof(AudioName)))
        {
            soundDictionary.Add(audioName, Array.Find(sounds, sound => sound.audioName == audioName));
        }
    }

    public async void WaitingSetMusicOnStopped(MusicName? musicName, bool isFaded = true)
    {
        Sound sound = currentMusicAudioName.HasValue ? soundDictionary[currentMusicAudioName.Value] : null;
        if (sound == null)
        {
            Debug.LogWarning($"Can't set music on stopped because sound is null");
            return;
        }
        if (sound.loop)
        {
            Debug.LogWarning($"Can't set music on stopped because sound is looped");
            return;
        }
        await UniTask.WaitUntil(() => !sound.source.isPlaying);
        SetMusic(musicName, isFaded);
    }
    public void SetMusicOutDuration(MusicName? musicName, out float remainingMusicDuration, bool isFaded = true)
    {
        SetMusic(musicName, out Sound sound, isFaded);
        remainingMusicDuration = sound != null ? sound.clip.length - sound.source.time : 0;
    }

    public void SetMusic(MusicName? musicName, out Sound sound, bool isFaded = true)
    {
        currentMusicAudioName = GetAudioNameByMusicAudioName(musicName);

        if (currentMusicName == musicName)
        {
            sound = currentMusicAudioName.HasValue ? soundDictionary[currentMusicAudioName.Value] : null;
            return;
        }

        currentMusicName = musicName;

        StopAudioType(AudioType.BGM, currentMusicAudioName, isFaded, 1);

        if (currentMusicAudioName.HasValue)
        {
            Play(currentMusicAudioName.Value, out sound, isFaded);
        }
        else
        {
            sound = null;
        }
    }

    public void SetMusic(MusicName? musicName, bool isFaded = true)
    {
        SetMusic(musicName, out _, isFaded);
    }

    private static readonly Dictionary<MusicName, AudioName> musicToAudioMap = Enum.GetValues(typeof(MusicName))
        .Cast<MusicName>()
        .Where(m => Enum.TryParse(m.ToString(), out AudioName result))
        .ToDictionary(m => m, m => (AudioName)Enum.Parse(typeof(AudioName), m.ToString()));

    public static AudioName? GetAudioNameByMusicAudioName(MusicName? musicAudioName)
    {
        if (musicAudioName == null) return null;
        return musicToAudioMap.TryGetValue(musicAudioName.Value, out var audioName) ? audioName : null;
    }

    public void Play(AudioName name, out Sound sound, bool isFaded = false, float fadedTime = 1)
    {
        sound = soundDictionary[name];

        if (sound == null)
        {
            Debug.LogWarning($"{name} data sound tidak ditemukan");
            return;
        }

        if (sound.isSingleUsed)
        {
            if (sound.source.isPlaying)
            {
                return;
            }
        }

        if (isFaded)
        {
            sound.source.volume = 0;
            StartCoroutine(FadedIn(sound, fadedTime));
            return;
        }
        sound.source.volume = 1;
        sound.source.Play();
    }
    public void Play(AudioName name, bool isFaded = false, float fadedTime = 1)
    {
        Play(name, out _, isFaded, fadedTime);
    }

    public AudioClip GetAudioClip(AudioName name)
    {
        Sound sound = soundDictionary[name];

        if (sound == null)
        {
            Debug.LogWarning("Data sound tidak ditemukan");
            return null;
        }

        return sound.source.clip;
    }

    public void PitchModifier(AudioName name, float pitch)
    {
        Sound sound = soundDictionary[name];

        if (sound == null)
        {
            return;
        }

        StartCoroutine(PitchSmootherModifier(sound.source, pitch));
    }

    IEnumerator PitchSmootherModifier(AudioSource source, float toPitch, float duration = 3f)
    {
        float currentPitch = source.pitch;
        float elapsedTime = 0.1f;
        while (elapsedTime < duration)
        {
            float modifierPitch = Mathf.Lerp(currentPitch, toPitch, elapsedTime / duration);
            source.pitch = modifierPitch;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.pitch = toPitch;
    }

    public void PlayOnShot(AudioName name)
    {
        Sound sound = soundDictionary[name];

        if (sound == null)
        {
            Debug.LogWarning("Data sound tidak ditemukan");
            return;
        }

        sound.source.PlayOneShot(sound.source.clip);
    }

    public void Stop(AudioName name, bool isFaded = false, float fadedTime = 1f)
    {
        Sound sound = soundDictionary[name];
        if (sound == null)
        {
            Debug.LogWarning("Data sound tidak ditemukan");
            return;
        }
        if (isFaded)
        {
            sound.source.volume = 1;
            StartCoroutine(FadedOut(sound, fadedTime));
            return;
        }
        sound.source.Stop();
    }

    public void StopAudioType(AudioType type, AudioName? exceptAudioName = null, bool isFaded = false, float fadedTime = 1f)
    {
        Sound[] soundAll = Array.FindAll(sounds, sound => sound.audioType == type);
        if (soundAll == null)
        {
            return;
        }
        foreach (var sound in soundAll)
        {
            if (exceptAudioName.HasValue)
            {
                if (sound.audioName == exceptAudioName.Value)
                {
                    continue;
                }
            }
            Stop(sound.audioName, isFaded, fadedTime);
        }
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    private IEnumerator FadedOut(Sound sound, float fadedTime)
    {
        float elapsedTime = 0;
        bool isAbort = false;
        while (elapsedTime <= fadedTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(1, 0, elapsedTime / fadedTime);

            if (GetAudioNameByMusicAudioName(currentMusicName) == sound.audioName)
            {
                sound.source.volume = 1;
                isAbort = true;
                break;
            }
        }

        if (!isAbort)
        {
            sound.source.Stop();
        }
    }

    private IEnumerator FadedIn(Sound sound, float fadedTime)
    {
        sound.source.Play();
        float elapsedTime = 0;
        while (elapsedTime <= fadedTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(0, 1, elapsedTime / fadedTime);
        }
    }

    public AudioSource AddAudio2D(GameObject addedObject)
    {
        AudioSource audioSource = addedObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.playOnAwake = false;
        audioSource.maxDistance = 15;
        audioSource.outputAudioMixerGroup = audioMixerGroupData.SFX;

        return audioSource;
    }

    public AudioSource AddAudio2D(GameObject addedObject, AudioClip audioClip)
    {
        AudioSource audioSource = AddAudio2D(addedObject);
        audioSource.clip = audioClip;

        return audioSource;
    }

    public AudioSource AddAudio3D(GameObject addedObject)
    {
        AudioSource audioSource = addedObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.playOnAwake = false;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.maxDistance = 15;
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, customRollOffCurve);
        audioSource.outputAudioMixerGroup = audioMixerGroupData.SFX;

        return audioSource;
    }

    public AudioSource AddAudio3D(GameObject addedObject, AudioClip audioClip)
    {
        AudioSource audioSource = AddAudio3D(addedObject);
        audioSource.clip = audioClip;

        return audioSource;
    }

    [System.Serializable]
    public struct AudioMixerGroupData
    {
        public AudioMixerGroup master;
        public AudioMixerGroup SFX;
        public AudioMixerGroup BGM;
        public AudioMixerGroup Voice;
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public AudioName audioName;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop;
    public bool isSingleUsed;
    public bool bypassEffect;
    public AudioType audioType;

    [HideInInspector]
    public AudioSource source;
}

public enum AudioType
{
    BGM,
    SFX,
    VOICE,
}

public enum AudioName
{
    BGM_MAINMENU_MAIN,
    BGM_GAMEPLAY_0,
    BGM_GAMEPLAY_0_END,
    SFX_CLICK,
    SFX_COIN,
    SFX_HEAL,
    SFX_THREE,
    SFX_TWO,
    SFX_ONE,
    SFX_FANFARE
}

public enum MusicName
{
    BGM_MAINMENU_MAIN,
    BGM_GAMEPLAY_0,
    BGM_GAMEPLAY_0_END
}