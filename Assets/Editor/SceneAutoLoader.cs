using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        const string PreviousSceneKey = "SceneAutoLoader.PreviousScene";

        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        static string BootstrapScenePath
        {
            get
            {
                var scenes = EditorBuildSettings.scenes;
                if (scenes.Length == 0) return null;
                return scenes[0].path;
            }
        }

        static void OnPlayModeChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    
                    var bootstrapPath = BootstrapScenePath;
                    if (string.IsNullOrEmpty(bootstrapPath)) return;

                    var currentScene = EditorSceneManager.GetActiveScene();
                    
                    EditorPrefs.SetString(PreviousSceneKey, currentScene.path);
                    
                    if (currentScene.path != bootstrapPath)
                    {
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                        EditorSceneManager.OpenScene(bootstrapPath, OpenSceneMode.Single);
                    }
                    
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    
                    var previousScene = EditorPrefs.GetString(PreviousSceneKey, "");
                    
                    if (!string.IsNullOrEmpty(previousScene) && previousScene != BootstrapScenePath)
                        EditorSceneManager.OpenScene(previousScene, OpenSceneMode.Single);
                    
                    break;
            }
        }
    }
}