using System.Collections.Generic;
using System.Linq;
using SceneManagement.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SceneManagement.EditorAddons
{
    /// <summary>
    /// Validator class for <seealso cref="SceneLoaderSettings"/>
    /// </summary>
    public static class SceneLoaderSettingsValidator
    {
        [DidReloadScripts]
        private static void ValidateOnDidReload()
        {
            if (Validate(out var settings)) return;
            Debug.LogError(ReturnErrorText(settings));
        }

        public static string ReturnErrorText(SceneLoaderSettings settings)
        {
            var list = settings == null ? new List<SceneLoaderAsset>() : settings.Scenes;
            return (settings, list) switch
                   {
                       (null, _) => $"{nameof(SceneLoaderSettings)} missing",
                       (_, null) => $"Scenes is empty in {nameof(SceneLoaderSettings)}",
                       (_, { } subList) when subList.Count <= 0 => $"Scenes is empty in {nameof(SceneLoaderSettings)}",
                       _ =>
                           $"Detect difference between {nameof(SceneLoaderSettings)} and scenes in Build Settings. Updating..."
                   };
        }

        public static bool Validate(out SceneLoaderSettings settings)
        {
            settings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
            return settings != null && ValidateScenesLoaderSettings(settings);
        }

        public static bool ValidateScenesLoaderSettings(SceneLoaderSettings settings)
        {
            var scenes = settings.Scenes;
            if (scenes == null) return false;

            for (var i = scenes.Count - 1; i >= 0; i--)
            {
                if (scenes[i] == null || !scenes[i].Validate()) continue;
                if (scenes.Count(x => x != null && x.Validate() && x.Guid == scenes[i].Guid) <= 1) continue;
                scenes[i] = null;
            }

            if (settings.IntermediateScene != null && settings.IntermediateScene.Validate())
                scenes.Remove(settings.IntermediateScene);
            
            settings.ResetSceneList(scenes);
            var sceneLoaderAssets = settings.Scenes.Where(x => x != null && x.Validate()).ToList();

            if (settings.IntermediateScene != null && settings.IntermediateScene.Validate())
                sceneLoaderAssets.Add(settings.IntermediateScene);

            var newScenes = sceneLoaderAssets
                           .Select(loaderAsset => new EditorBuildSettingsScene(loaderAsset.FullPath, true)).ToArray();
            var editorBuildSettingsScenes = EditorBuildSettings.scenes;
            var valid = editorBuildSettingsScenes.SequenceEqual(newScenes, new EditorSceneComparer());
            if (!valid) EditorBuildSettings.scenes = newScenes;
            return valid;
        }
    }
}
