using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextMeshProUGUITranslator), editorForChildClasses: true)]
public class TextMeshProUGUITranslatorEditor : Editor
{
    TextMeshProUGUITranslator textMeshProUGUITranslator;
    SerializedProperty stringId;

    private void OnEnable()
    {
        stringId = serializedObject.FindProperty("stringId");

        textMeshProUGUITranslator = target as TextMeshProUGUITranslator;
        if (!Application.isPlaying)
        {
            textMeshProUGUITranslator.text = $"[{textMeshProUGUITranslator.stringId}]";
            name = $"{textMeshProUGUITranslator.stringId}LabelText";
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (!Application.isPlaying)
        {
            textMeshProUGUITranslator.text = $"[{textMeshProUGUITranslator.stringId}]";
            name = $"{textMeshProUGUITranslator.stringId}Text";
        }

        base.OnInspectorGUI();
    }
}
