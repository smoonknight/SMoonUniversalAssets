public class SceneHelper
{
    static public string GetSceneBySceneManagerEnum(SceneManagerEnum sceneManagerEnum)
    {
        return sceneManagerEnum switch
        {
            SceneManagerEnum.MAINMENU => "Scene_Main Menu",
            SceneManagerEnum.GAMEPLAY_HOUSE => "Scene_Gameplay_House",
            SceneManagerEnum.GAMEPLAY_SCHOOL => "Scene_Gameplay_School",
            SceneManagerEnum.GAMEPLAY_BATTLE => "Scene_Gameplay_Battle",
            _ => throw new System.ArgumentOutOfRangeException(nameof(sceneManagerEnum), $"Unhandled sceneManagerEnum: {sceneManagerEnum}")
        };
    }
}

public enum SceneManagerEnum
{
    MAINMENU, GAMEPLAY_HOUSE, GAMEPLAY_SCHOOL, GAMEPLAY_BATTLE
}