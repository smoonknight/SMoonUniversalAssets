using UnityEngine;

namespace SMoonUniversalAsset
{
    public class GameManager : SingletonWithDontDestroyOnLoad<GameManager>
    {
        public GameInfoScriptableObject gameInfo;

        public string GetPlayerName() => "Hisa";
        public string GameVersion => gameInfo.gameVersion;

        public void InitAllAction()
        {

        }
    }

    public struct PlayerStatus
    {

    }
}