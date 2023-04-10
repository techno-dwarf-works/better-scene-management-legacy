using System.IO;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    internal static class BetterInternalTools
    {
        public const string BetterPrefix = nameof(Better);
        public const string NamespacePrefix = nameof(SceneManagement);

        private static readonly string[] FolderPaths = new string[]
            { BetterPrefix, NamespacePrefix, nameof(Resources) };

        public const string MenuItemPrefix = BetterPrefix + "/Scene Management";
        
        private static string GenerateResourcesRelativePath()
        {
            return Path.Combine(FolderPaths);
        }
        
        public static T LoadOrCreateScriptableObject<T>() where T : ScriptableObject
        {
            var name = typeof(T).Name;
            var settings = Resources.Load<T>(name);
            if (settings != null) return settings;

            settings = ScriptableObject.CreateInstance<T>();

            var relativePath = GenerateResourcesRelativePath();
            var absolutePath = Path.Combine(Application.dataPath, relativePath);

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
            
            relativePath = Path.Combine("Assets", relativePath, $"{name}.asset");
            AssetDatabase.CreateAsset(settings, relativePath);
            return settings;
        }
        
    }
}