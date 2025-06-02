using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumAsIntAttribute))]
public class EnumAsIntDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumAsIntAttribute enumAttribute = (EnumAsIntAttribute)attribute;

        if (enumAttribute.EnumType.IsEnum)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUILayout.BeginHorizontal();
            int enumValue = property.enumValueIndex;
            int newValue = EditorGUI.IntField(position, label.text, enumValue);

            Enum enumObject = (Enum)Enum.ToObject(enumAttribute.EnumType, newValue);
            GUILayout.Label(enumObject.ToString(), EditorStyles.label);
            GUILayout.EndHorizontal();

            if (System.Enum.IsDefined(enumAttribute.EnumType, newValue))
            {
                property.enumValueIndex = newValue;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Invalid Enum Value");
            }

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use EnumAsInt with enums only.");
        }
    }
}


