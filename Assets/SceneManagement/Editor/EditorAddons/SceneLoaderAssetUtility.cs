using SceneManagement.Runtime;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace SceneManagement.EditorAddons
{
    public static class SceneLoaderAssetUtility
    {
        public static SceneLoaderAsset ToSceneLoaderAsset(this SceneAsset scene)
        {
            var path = AssetDatabase.GetAssetPath(scene);
            var guid = AssetDatabase.AssetPathToGUID(path);
            return new SceneLoaderAsset(path, guid);
        }
    }
}