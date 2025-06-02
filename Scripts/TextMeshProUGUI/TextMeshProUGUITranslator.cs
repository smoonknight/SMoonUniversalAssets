using Cysharp.Threading.Tasks;
using SMoonUniversalAsset;
using TMPro;
using UnityEngine;

public class TextMeshProUGUITranslator : TextMeshProUGUIOnTranslateBase
{
    [EnumIncrementDecrement(typeof(StringId))]
    public StringId stringId;

    public async override void Translate()
    {
        text = "";
        await UniTask.WaitUntil(() => LanguageManager.Instance != null);
        text = stringId.ToCommonLanguage();
    }

    public void Translate(StringId stringId)
    {
        this.stringId = stringId;
        Translate();
    }
}