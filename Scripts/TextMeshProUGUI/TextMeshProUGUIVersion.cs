using UnityEngine;

public class TextMeshProUGUIVersion : TextMeshProUGUIOnTranslateBase
{
    public GameInfoScriptableObject gameInfo;
    public override void Translate()
    {
        text = gameInfo.gameVersion;
    }
}