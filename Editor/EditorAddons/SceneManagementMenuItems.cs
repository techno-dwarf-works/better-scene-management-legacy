using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    public static class SceneManagementMenuItems
    {
        [MenuItem("Better/Scene Management/Highlight Scene Settings", false)]
        private static void Highlight()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            if (settings != null)
                Selection.SetActiveObjectWithContext(settings, settings);
        }

        [MenuItem("Better/Scene Management/Highlight Scene Settings", true)]
        private static bool HighlightValidate()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            return settings != null;
        }

        [MenuItem("Better/Scene Management/Create Scene Settings", false)]
        private static void CreateNewSceneLoaderSettings()
        {
            SceneSettingsValidator.LoadOrCreateSettings();
        }

        [MenuItem("Better/Scene Management/Create Scene Settings", true)]
        private static bool CreateNewSceneLoaderSettingsValidate()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            return settings == null;
        }
    }
}