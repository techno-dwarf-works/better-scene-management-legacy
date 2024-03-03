using System.Linq;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    [InitializeOnLoad]
    public static class ScenesValidator
    {
        static ScenesValidator()
        {
            EditorApplication.delayCall -= OnEditorApplicationCall;
            EditorApplication.delayCall += OnEditorApplicationCall;
            EditorBuildSettings.sceneListChanged -= OnSceneListChanged;
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        public static void ValidateSettings()
        {
            SceneSystemSettings.Instance.Validate();
        }

        public static bool ValidateSceneInBuildSettings(SceneReference sceneReference)
        {
            if (sceneReference == null || !sceneReference.Validate())
            {
                return false;
            }

            var editorBuildSettingsName = nameof(EditorBuildSettings).PrettyCamelCase();
            var scene = EditorBuildSettings.scenes.FirstOrDefault(x => x.path.CompareOrdinal(sceneReference.FullPath));
            if (scene == null)
            {
                Debug.LogWarning($"{sceneReference.Name} added in {editorBuildSettingsName}");
                var intermediateEditor = new EditorBuildSettingsScene(sceneReference.FullPath, true);
                var newScenes = EditorBuildSettings.scenes.Append(intermediateEditor).ToArray();
                EditorBuildSettings.scenes = newScenes;
            }
            else if (!scene.enabled)
            {
                Debug.LogWarning($"{sceneReference.Name} enabled in {editorBuildSettingsName}");
                scene.enabled = true;
            }

            return true;
        }

        private static void OnEditorApplicationCall()
        {
            ValidateSettings();
        }

        private static void OnSceneListChanged()
        {
            ValidateSettings();
        }
    }
}