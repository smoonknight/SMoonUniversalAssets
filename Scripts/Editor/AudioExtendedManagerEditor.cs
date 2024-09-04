using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioExtendedManager))]
public class AudioExtendedManagerEditor : Editor
{
    SerializedProperty soundsProperty;
    Dictionary<AudioName, Sound> soundDictionary;

    void OnEnable()
    {
        soundsProperty = serializedObject.FindProperty("sounds");
        InitializeSoundDictionary();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.LabelField("Audio Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        foreach (AudioName audioName in Enum.GetValues(typeof(AudioName)))
        {
            if (!soundDictionary.ContainsKey(audioName))
            {
                EditorGUILayout.HelpBox($"Missing Sound: {audioName}", MessageType.Warning);
                if (GUILayout.Button($"Add {audioName}"))
                {
                    AddSound(audioName);
                }
            }
            else
            {
                DisplaySoundEditor(soundDictionary[audioName]);
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty((AudioExtendedManager)target);
            AssetDatabase.SaveAssets();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<AudioName, Sound>();
        AudioExtendedManager manager = (AudioExtendedManager)target;

        foreach (Sound sound in manager.sounds)
        {
            if (!soundDictionary.ContainsKey(sound.audioName))
            {
                soundDictionary.Add(sound.audioName, sound);
            }
        }
    }

    private void AddSound(AudioName audioName)
    {
        AudioExtendedManager manager = (AudioExtendedManager)target;
        Sound newSound = new Sound { audioName = audioName };
        ArrayUtility.Add(ref manager.sounds, newSound);
        soundDictionary.Add(audioName, newSound);
        EditorUtility.SetDirty(target);
    }

    private void DisplaySoundEditor(Sound sound)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(sound.audioName.ToString(), EditorStyles.boldLabel);
        sound.clip = (AudioClip)EditorGUILayout.ObjectField("Clip", sound.clip, typeof(AudioClip), false);
        sound.pitch = EditorGUILayout.Slider("Pitch", sound.pitch, 0.1f, 3f);
        sound.loop = EditorGUILayout.Toggle("Loop", sound.loop);
        sound.isSingleUsed = EditorGUILayout.Toggle("Single Use", sound.isSingleUsed);
        sound.audioType = (AudioType)EditorGUILayout.EnumPopup("Audio Type", sound.audioType);
        EditorGUILayout.EndVertical();
    }
}