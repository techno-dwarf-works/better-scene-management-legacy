using System.Linq;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    /// <summary>
    /// Validator class for <seealso cref="SceneLoaderSettings"/>
    /// </summary>
    public static class SceneValidator
    {
        public static void ValidateSceneInBuildSettings(SceneLoaderAsset sceneLoaderAsset)
        {
            var scene = EditorBuildSettings.scenes.FirstOrDefault(x => x.path.FastEquals(sceneLoaderAsset.FullPath));
            if (!sceneLoaderAsset.Validate()) return;
            var editorBuildSettingsName = nameof(EditorBuildSettings).PrettyCamelCase();
            if (scene == null)
            {
                Debug.LogWarning($"{sceneLoaderAsset.Name} missing in {editorBuildSettingsName}. Adding...");
                var intermediateEditor = new EditorBuildSettingsScene(sceneLoaderAsset.FullPath, true);
                var newScenes = EditorBuildSettings.scenes.Append(intermediateEditor).ToArray();
                EditorBuildSettings.scenes = newScenes;
            }
            else if (!scene.enabled)
            {
                Debug.LogWarning($"{sceneLoaderAsset.Name} disabled in {editorBuildSettingsName}. Updating...");
                scene.enabled = true;
            }
        }
    }
}