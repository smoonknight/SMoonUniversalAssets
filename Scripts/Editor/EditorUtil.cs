#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class EditorUtil
{
    public static string gameplayScenePath = "Assets/Handmad_assets/Scene/Gameplay/";
    public static string StoragePath = "Assets/Handmad_assets/Storage/";

    // static public bool IsCurrentSceneSameWithWorldName(WorldName worldName)
    // {
    //     Scene activedScene = SceneManager.GetActiveScene();
    //     return SceneHelper.GetSceneByWorldName(worldName) == activedScene.name;
    // }

    static public bool IsCurrentSceneGameplayScene()
    {
        Scene activedScene = SceneManager.GetActiveScene();
        return activedScene.name switch
        {
            "Scene_Gameplay_House" => true,
            "Scene_Gameplay_Alley" => true,
            "Scene_Gameplay_Chicken Coop" => true,
            _ => false
        };
    }

    static public string[] GetSceneNames()
    {
        string[] strings = new string[] {
            "Scene_Gameplay_House",
            "Scene_Gameplay_Alley",
            "Scene_Gameplay_Chicken Coop"
        };
        return strings;
    }


    // static public void OpenSceneOnEditor(WorldName worldName) => EditorSceneManager.OpenScene($"{gameplayScenePath}{SceneHelper.GetSceneByWorldName(worldName)}.unity");
}
#endif