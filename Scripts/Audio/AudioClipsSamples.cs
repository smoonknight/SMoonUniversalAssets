using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class AudioClipSamples
{
    public List<AudioClip> audioClips;
    [ReadOnly]
    public int currentIndex = 0;

    public bool IsHaveSample() => audioClips.Count != 0;

    public AudioClip GetClip()
    {
        if (currentIndex == 0)
        {
            audioClips.Randomize();
        }

        AudioClip audioClip = audioClips[currentIndex];

        currentIndex++;

        if (currentIndex >= audioClips.Count)
        {
            currentIndex = 0;
        }

        return audioClip;
    }
}