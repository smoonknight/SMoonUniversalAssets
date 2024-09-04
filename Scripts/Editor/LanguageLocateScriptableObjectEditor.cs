using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(LanguageLocateScriptableObject))]
public class LanguageLocateScriptableObjectEditor : Editor
{
    private SerializedProperty indonesiaLanguageLocateData;
    private SerializedProperty englishLanguageLocateData;

    private void OnEnable()
    {
        indonesiaLanguageLocateData = serializedObject.FindProperty("indonesiaLanguageLocateData");
        englishLanguageLocateData = serializedObject.FindProperty("englishLanguageLocateData");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawLanguageLocateData(indonesiaLanguageLocateData, "Indonesian Language Data");
        DrawLanguageLocateData(englishLanguageLocateData, "English Language Data");

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLanguageLocateData(SerializedProperty languageLocateData, string label)
    {
        EditorGUILayout.PropertyField(languageLocateData, new GUIContent(label), false);
        if (languageLocateData.isExpanded)
        {
            EditorGUI.indentLevel++;

            DrawLanguageDataSection(languageLocateData.FindPropertyRelative("commonLanguageDatas"), typeof(StringId), "Common Language Data", "stringId");

            EditorGUI.indentLevel--;
        }
    }

    private void DrawLanguageDataSection(SerializedProperty list, Type enumType, string label, string primaryName)
    {
        EditorGUILayout.PropertyField(list, new GUIContent(label), true);
        EnsureAllEnumsInLanguageData(list, enumType, primaryName);

        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty item = list.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(item.FindPropertyRelative(primaryName), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(item.FindPropertyRelative("text"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
    }

    private void EnsureAllEnumsInLanguageData(SerializedProperty list, Type enumType, string primaryName)
    {
        HashSet<int> existingIds = new HashSet<int>();
        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty item = list.GetArrayElementAtIndex(i);
            int id = item.FindPropertyRelative(primaryName).enumValueIndex;
            existingIds.Add(id);
        }

        foreach (int id in Enum.GetValues(enumType))
        {
            if (!existingIds.Contains(id))
            {
                list.arraySize++;
                SerializedProperty newItem = list.GetArrayElementAtIndex(list.arraySize - 1);
                newItem.FindPropertyRelative(primaryName).enumValueIndex = id;
                newItem.FindPropertyRelative("text").stringValue = string.Empty;
            }
        }
    }
}