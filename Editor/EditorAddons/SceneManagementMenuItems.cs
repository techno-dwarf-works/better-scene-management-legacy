using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    public static class SceneManagementMenuItems
    {
        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Highlight Scene Settings", false)]
        private static void Highlight()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            if (settings != null)
                Selection.SetActiveObjectWithContext(settings, settings);
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Highlight Scene Settings", true)]
        private static bool HighlightValidate()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            return settings != null;
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Create Scene Settings", false)]
        private static void CreateNewSceneLoaderSettings()
        {
            SceneSettingsValidator.ValidateSettings();
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Create Scene Settings", true)]
        private static bool CreateNewSceneLoaderSettingsValidate()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            return settings == null;
        }
    }
}