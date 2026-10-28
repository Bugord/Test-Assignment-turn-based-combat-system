using UnityEditor;
using UnityEditor.SceneManagement;

namespace Game.Editor
{
    [InitializeOnLoad]
    public static class BootScenePlayMode
    {
        static BootScenePlayMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Apply();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingEditMode) {
                Apply();
            }
        }

        private static void Apply()
        {
            var scenes = EditorBuildSettings.scenes;
            EditorSceneManager.playModeStartScene = scenes.Length > 0
                ? AssetDatabase.LoadAssetAtPath<SceneAsset>(scenes[0].path)
                : null;
        }
    }
}
