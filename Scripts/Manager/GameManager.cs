using System;
using UnityEngine;

public partial class GameManager : SingletonWithDontDestroyOnLoad<GameManager>
{
    public GameInfoScriptableObject gameInfo;

    public string GetPlayerName() => "Lorem";
    public string GameVersion => gameInfo.gameVersion;

    public void InitAllAction()
    {

    }

    /// <summary>
    /// "SetCursor is currently bugged on mobile platforms. Cursor will always be visible On Android (visible = true) regardless of the isShow value."
    /// </summary>
    /// <param name="isShow">show cursor and setlockmode none</param>
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