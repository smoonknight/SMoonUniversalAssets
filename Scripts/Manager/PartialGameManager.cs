using System;
using UnityEngine;

public partial class GameManager : SingletonWithDontDestroyOnLoad<GameManager>
{
    public GameInfoScriptableObject gameInfo;

    public string GetPlayerName() => "Hisa";
    public string GameVersion => gameInfo.gameVersion;

    public void InitAllAction()
    {

    }

    [Obsolete("SetCursor is currently bugged on mobile platforms. Cursor will always be visible On Android (visible = true) regardless of the isShow value.")]
    public void SetCursor(bool isShow)
    {
#if !UNITY_STANDALONE_WIN
        isShow = true;
#endif
        Cursor.visible = isShow;
        Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;
    }
}

public struct PlayerStatus
{

}