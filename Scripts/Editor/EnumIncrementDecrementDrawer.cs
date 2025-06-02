using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumIncrementDecrementAttribute))]
public class EnumIncrementDecrementDrawer : PropertyDrawer
{
    int inputIndex = -1;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            EnumIncrementDecrementAttribute enumAttribute = (EnumIncrementDecrementAttribute)attribute;
            Type enumType = enumAttribute.EnumType;

            var enumLength = Enum.GetValues(enumType).Length;

            if (property.enumValueIndex >= 0 && property.enumValueIndex < enumLength)
            {
                EditorGUI.BeginProperty(position, label, property);

                float buttonWidth = 20f;
                float spacing = 2f;
                float textWidth = 40f;
                float labelWidth = position.width - buttonWidth * 2 - spacing * 3 - textWidth;

                // Label enum
                Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
                EditorGUI.LabelField(labelRect, label, new GUIContent(property.enumDisplayNames[property.enumValueIndex]));

                float textX = position.x + labelWidth + spacing;
                Rect textRect = new Rect(textX, position.y, textWidth, position.height);

                // Text field untuk index
                inputIndex = EditorGUI.IntField(textRect, property.enumValueIndex);
                inputIndex = Mathf.Clamp(inputIndex, 0, enumLength - 1);
                if (inputIndex != property.enumValueIndex)
                {
                    property.enumValueIndex = inputIndex;
                }


                float buttonX1 = textX + textWidth + spacing;
                float buttonX2 = buttonX1 + buttonWidth + spacing;

                // Tombol naik (▲)
                Rect buttonUpRect = new Rect(buttonX1, position.y, buttonWidth, position.height);
                GUI.enabled = property.enumValueIndex < enumLength - 1;
                if (GUI.Button(buttonUpRect, "▲"))
                {
                    property.enumValueIndex++;
                }

                // Tombol turun (▼)
                Rect buttonDownRect = new Rect(buttonX2, position.y, buttonWidth, position.height);
                GUI.enabled = property.enumValueIndex > 0;
                if (GUI.Button(buttonDownRect, "▼"))
                {
                    property.enumValueIndex--;
                }

                inputIndex = property.enumValueIndex;

                GUI.enabled = true;
                EditorGUI.EndProperty();
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }


}
