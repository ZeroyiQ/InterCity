using UnityEditor;
using UnityEditor.SceneManagement;

namespace InterCity.Editor
{
    public partial class MyTools
    {
        private const string LAUNCHER_SCENES = "Assets/Launcher.unity";
        private const string MENU_SCENES = "Assets/GameMain/Scenes/Demo/Menu.unity";
        private const string LEVEL_SCENES = "Assets/GameMain/Scenes/Level.unity";

        [MenuItem("MyTools/Scenes/Launch", false, 1)]
        private static void OpenLauncherScene()
        {
            EditorSceneManager.OpenScene(LAUNCHER_SCENES);
        }

        [MenuItem("MyTools/Scenes/Menu", false, 2)]
        private static void OpenMenuScene()
        {
            EditorSceneManager.OpenScene(MENU_SCENES);
        }

        [MenuItem("MyTools/Scenes/Level", false, 3)]
        private static void OpenLevelScene()
        {
            EditorSceneManager.OpenScene(LEVEL_SCENES);
        }

    }
}
