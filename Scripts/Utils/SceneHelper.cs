using System;
using SMoonUniversalAsset;
using UnityEngine.SceneManagement;

public class SceneHelper
{
    static public string GetSceneBySceneEnum(SceneEnum sceneManagerEnum)
    {
        return sceneManagerEnum switch
        {
            SceneEnum.MAINMENU => "Scene_MainMenu",
            SceneEnum.GAMEPLAY_ROGUE => "Scene_Rogue",
            SceneEnum.GAMEPLAY_DEBUG => "Scene_Debug",
            _ => throw new System.ArgumentOutOfRangeException(nameof(sceneManagerEnum), $"Unhandled sceneManagerEnum: {sceneManagerEnum}")
        };
    }

    public static SceneEnum GetCurrentSceneEnum() => GetSceneEnumByString(SceneManager.GetActiveScene().name);

    public static SceneEnum GetSceneEnumByString(string sceneName)
    {
        return sceneName switch
        {
            "Scene_MainMenu" => SceneEnum.MAINMENU,
            "Scene_Rogue" => SceneEnum.GAMEPLAY_ROGUE,
            "Scene_Debug" => SceneEnum.GAMEPLAY_DEBUG,
            _ => throw new System.ArgumentOutOfRangeException(nameof(sceneName), $"Unhandled sceneName: {sceneName}")
        };
    }

    static public void CheckCurrentSceneRequire()
    {
        var selectedSceneManagerEnum = GetCurrentSceneEnum();
        CheckSceneRequire(selectedSceneManagerEnum);
    }

    static public void CheckSceneRequire(SceneEnum sceneManagerEnum)
    {

    }
}

public enum SceneEnum
{
    MAINMENU, GAMEPLAY_ROGUE, GAMEPLAY_DEBUG
}