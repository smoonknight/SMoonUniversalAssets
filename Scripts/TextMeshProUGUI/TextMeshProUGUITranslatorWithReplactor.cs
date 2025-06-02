using Cysharp.Threading.Tasks;
using SMoonUniversalAsset;

public class TextMeshProUGUITranslatorWithReplactor : TextMeshProUGUITranslator
{
    public async override void Translate()
    {
        await UniTask.WaitUntil(() => LanguageManager.Instance != null);
        text = LanguageManager.Instance.GetDictionaryTextReplactor(stringId);
    }
}