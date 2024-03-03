using System;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons.Extensions
{
    public static class SceneAssetExtensions
    {
        public static SceneReference ToSceneReference(this SceneAsset sceneAsset)
        {
            if (sceneAsset == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneAsset));
                return new();
            }

            var path = AssetDatabase.GetAssetPath(sceneAsset);
            var guid = AssetDatabase.AssetPathToGUID(path);
            return new(path, guid);
        }
    }
}