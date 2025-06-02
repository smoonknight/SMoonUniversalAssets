using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class AudioClipSamples
{
    public List<AudioClip> audioClips;
    List<int> availableIndices = new();

    private void InitializeIndices()
    {
        availableIndices.Clear();
        for (int i = 0; i < audioClips.Count; i++)
        {
            availableIndices.Add(i);
        }
    }

    public AudioClip GetRandomStepSound()
    {
        if (audioClips.Count == 0) return null;

        if (availableIndices.Count == 0)
        {
            InitializeIndices();
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        int clipIndex = availableIndices[randomIndex];
        availableIndices.RemoveAt(randomIndex);

        return audioClips[clipIndex];
    }
}