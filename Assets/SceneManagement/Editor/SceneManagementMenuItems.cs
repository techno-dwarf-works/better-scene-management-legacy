using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SceneManagement.Editor
{
    public static class SceneManagementMenuItems
    {
        [MenuItem("Scene Managment/Create Scene Settings", false, 10)]
        private static void CreateNewSceneLoaderSettings()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            if (settings != null) return;
            var pattern = Path.Combine(nameof(SceneManagement), nameof(Resources));
            var dataPath = Application.dataPath;
            var paths = Directory.GetDirectories(dataPath, pattern, SearchOption.TopDirectoryOnly);
            var pathToResources = Path.Combine(dataPath, nameof(SceneManagement), nameof(Resources));
            if (paths.Length < 1) Directory.CreateDirectory(pathToResources);
            var newSettings = ScriptableObject.CreateInstance<SceneLoaderSettings>();

            var relativePath = Path.Combine(GetRelativePath(dataPath, pathToResources),
                                            $"{nameof(SceneLoaderSettings)}.asset");
            AssetDatabase.CreateAsset(newSettings, relativePath);
        }

        private static string GetRelativePath(string relativeTo, string path)
        {
            var uri = new Uri(relativeTo);

            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString())
                         .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (!rel.Contains(Path.DirectorySeparatorChar.ToString())) rel = $".{Path.DirectorySeparatorChar}{rel}";
            return rel;
        }

        [MenuItem("Scene Management/Highlight Scene Settings", false, 9)]
        private static void Highlight()
        {
            var settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            Selection.SetActiveObjectWithContext(settings, settings);
        }
    }
}
