using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
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