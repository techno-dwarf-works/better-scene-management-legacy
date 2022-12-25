using System.IO;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    [InitializeOnLoad]
    public static class SceneSettingsValidator
    {
        private static string[] _folderPaths = new string[]
            { nameof(Better), nameof(SceneManagement), "Resources" };

        static SceneSettingsValidator()
        {
            ValidateOnDidReload();
            EditorBuildSettings.sceneListChanged -= OnSceneListChanged;
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        private static void OnSceneListChanged()
        {
            ValidateOnDidReload();
        }

        private static string GenerateRelativePath()
        {
            return Path.Combine(_folderPaths);
        }

        private static bool Validate(out SceneLoaderSettings settings)
        {
            settings = LoadOrCreateSettings();
            return settings != null;
        }

        public static SceneLoaderSettings LoadOrCreateSettings()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            if (settings != null) return settings;
            var relativePath = GenerateRelativePath();

            settings = ScriptableObject.CreateInstance<SceneLoaderSettings>();
            var absolutePath = Path.Combine(Application.dataPath, relativePath);

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            relativePath = Path.Combine("Assets", relativePath, $"{nameof(SceneLoaderSettings)}.asset");
            AssetDatabase.CreateAsset(settings, relativePath);
            return settings;
        }


        private static void ValidateOnDidReload()
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
            SceneValidator.ValidateSceneInBuildSettings(intermediate);
        }
    }
}