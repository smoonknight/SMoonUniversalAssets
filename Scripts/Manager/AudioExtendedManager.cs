using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class AudioExtendedManager : SingletonWithDontDestroyOnLoad<AudioExtendedManager>
{
    public AudioMixerGroupData audioMixerGroupData;
    public AnimationCurve customRollOffCurve;

    public Sound[] sounds;

    Dictionary<AudioName, Sound> soundDictionary = new();

    float VolumeMixer(float value) => Mathf.Lerp(-40f, 0f, value);
    float FocusMixer(float value) => Mathf.Lerp(-50f, 0f, value);
    float LowpassMixer(float value) => Mathf.Lerp(10, 22000, value);
    public void SetAudioMixerBGMVolume() => SetAudioMixerBGMVolume(PlayerPrefsManager.Instance.GetFloat(FloatPrefsEnum.Settings_Audio_BGM_Volume));
    public void SetAudioMixerBGMVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.BGM, volume);

    public void SetAudioMixerSFXVolume() => SetAudioMixerSFXVolume(PlayerPrefsManager.Instance.GetFloat(FloatPrefsEnum.Settings_Audio_SFX_Volume));
    public void SetAudioMixerSFXVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.SFX, volume);

    public void SetAudioMixerVoiceVolume() => SetAudioMixerVoiceVolume(PlayerPrefsManager.Instance.GetFloat(FloatPrefsEnum.Settings_Audio_Voice_Volume));
    public void SetAudioMixerVoiceVolume(float volume) => SetAudioMixerVolume(audioMixerGroupData.Voice, volume);

    public void SetAudioMixerLowpass(float value) => SetAudioMixerParam(audioMixerGroupData.BGM, LowpassMixer(value), "Lowpass BGM");

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

    protected override void Awake()
    {
        base.Awake();
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = 1;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = GetAudioMixerGroup(sound.audioType);
        }

        SetSoundDictionary();
    }

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

    private void Start()
    {
        SetAudioMixerBGMVolume();
        SetAudioMixerSFXVolume();
    }

    public void SetSoundDictionary()
    {
        foreach (AudioName audioName in Enum.GetValues(typeof(AudioName)))
        {
            soundDictionary.Add(audioName, Array.Find(sounds, sound => sound.audioName == audioName));
        }
    }

    public void Play(AudioName name, bool isFaded = false, float fadedTime = 1)
    {
        Sound sound = soundDictionary[name];

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

    public void StopAudioType(AudioType type)
    {
        Sound[] soundAll = Array.FindAll(sounds, sound => sound.audioType == type);
        if (soundAll == null)
        {
            return;
        }
        foreach (var sound in soundAll)
        {
            sound.source.Stop();
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
        while (elapsedTime <= fadedTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(1, 0, elapsedTime / fadedTime);
        }
        sound.source.Stop();
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
    BGM_BacksoundMusic
}

public enum AudioName
{
    BGM_MAINMENU,
    BGM_HOUSE_1,
    BGM_HOUSE_2,
    BGM_HOUSE_3,
    BGM_SCHOOL_1,
    BGM_SCHOOL_2,
    BGM_SCHOOL_3,
    BGM_BATTLE_1,
    BGM_BATTLE_2,
    BGM_BATTLE_3,
    SFX_SCHOOL_RING,
    SFX_SMALL_SHOT,
    SFX_SLASH,
    SFX_LASER,
    SFX_CLICK
}