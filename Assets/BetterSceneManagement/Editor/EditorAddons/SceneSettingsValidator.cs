using Better.EditorTools.SettingsTools;
using Better.SceneManagement.EditorAddons.Settings;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    [InitializeOnLoad]
    public static class SceneSettingsValidator
    {
        static SceneSettingsValidator()
        {
            ValidateSettings();
            EditorBuildSettings.sceneListChanged -= OnSceneListChanged;
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        private static void OnSceneListChanged()
        {
            ValidateSettings();
        }

        private static bool Validate(out SceneLoaderSettings settings)
        {
            settings = BetterSettingsToolsContainer<SceneSettingsTool>.Instance.LoadOrCreateScriptableObject();
            return settings != null;
        }

        public static void ValidateSettings()
        {
            if (Validate(out var settings))
            {
                ValidateScenesSettings(settings);
            }
            else
            {
                Debug.LogWarning($"{nameof(SceneLoaderSettings)} missing");
            }
        }

        public static void ValidateScenesSettings(SceneLoaderSettings settings)
        {
            var intermediate = settings.IntermediateScene;
            if (intermediate.Validate())
                SceneValidator.ValidateSceneInBuildSettings(intermediate);
        }
    }
}