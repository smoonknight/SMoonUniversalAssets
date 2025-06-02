using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MultiLabelView : ViewBase
{
    [SerializeField]
    protected List<TextMeshProUGUI> labelTexts;

    public void Initialize(params string[] texts)
    {
        if (texts.Length > labelTexts.Count())
        {
            Debug.LogWarning($"text yang diminta tidak boleh lebih dari label text yang tersedia pada {name}");
            return;
        }

        labelTexts.Select(select => select.text = "");
        for (int i = 0; i < texts.Length; i++)
        {
            labelTexts[i].text = texts[i];
        }
    }
}