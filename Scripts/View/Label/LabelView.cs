using TMPro;
using UnityEngine;

public class LabelView : ViewBase
{
    [SerializeField]
    protected TextMeshProUGUI labelText;

    public void Initialize(string text)
    {
        labelText.text = text;
    }
}