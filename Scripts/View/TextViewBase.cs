using TMPro;
using UnityEngine;

public class TextViewBase : ViewBase
{
    [SerializeField]
    protected TextMeshProUGUI labelText;

    public void ChangeText(string text) => labelText.text = text;
    public bool TryInitializeTranslator(StringId stringId)
    {
        if (labelText is TextMeshProUGUITranslator translatorLabelText)
        {
            translatorLabelText.Translate(stringId);
            return true;
        }
        return false;
    }
}