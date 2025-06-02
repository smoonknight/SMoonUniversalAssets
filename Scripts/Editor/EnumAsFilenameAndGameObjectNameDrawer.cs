using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumAsFilenameAndGameObjectNameAttribute))]
public class EnumAsFilenameAndGameObjectNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)
            return;
        if (property.propertyType != SerializedPropertyType.Enum)
        {
            EditorGUI.LabelField(position, label.text, "Use EnumAsFilename with enums only.");
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        // Tampilkan enum popup
        Rect enumRect = new Rect(position.x, position.y, position.width - 70, position.height);
        property.enumValueIndex = EditorGUI.Popup(enumRect, label.text, property.enumValueIndex, property.enumDisplayNames);

        // Tombol rename
        Rect buttonRenameRect = new Rect(position.x + position.width - 65, position.y, 60, position.height);
        if (GUI.Button(buttonRenameRect, "Rename"))
        {
            string selectedEnumName = property.enumDisplayNames[property.enumValueIndex];
            UnityEngine.Object targetObject = property.serializedObject.targetObject;

            if (targetObject is GameObject gameObject)
            {
                RenameGameObject(gameObject, selectedEnumName);
            }
            else if (IsAsset(targetObject, out string assetPath))
            {
                RenameAsset(assetPath, selectedEnumName);
            }
            else
            {
                Debug.LogWarning("Unsupported target for renaming.");
            }
        }
        EditorGUI.EndProperty();
    }

    private void RenameGameObject(GameObject gameObject, string newName)
    {
        gameObject.name = newName;
        Debug.Log($"GameObject renamed to: {newName}");
    }

    private bool IsAsset(UnityEngine.Object target, out string assetPath)
    {
        assetPath = AssetDatabase.GetAssetPath(target);
        return !string.IsNullOrEmpty(assetPath);
    }

    private void RenameAsset(string assetPath, string newName)
    {
        string directory = Path.GetDirectoryName(assetPath);
        string newAssetPath = Path.Combine(directory, $"{newName}{Path.GetExtension(assetPath)}");

        AssetDatabase.RenameAsset(assetPath, newName);
        AssetDatabase.SaveAssets();
        Debug.Log($"Asset renamed to: {newAssetPath}");
    }
}
