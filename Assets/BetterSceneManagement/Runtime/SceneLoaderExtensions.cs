using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    /// <summary>
    /// Extensions for <seealso cref="SceneLoader"/>
    /// </summary>
    public static class SceneLoaderExtensions
    {
        private const int Seconds = 1000;

        /// <summary>
        /// Unloads scene by SceneLoaderAsset
        /// </summary>
        /// <param name="sceneReference"></param>
        /// <param name="mode"></param>
        /// <param name="autoSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        internal static async Task SceneUnloadOperation(this SceneReference sceneReference, UnloadSceneOptions mode, bool autoSwitch,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var scene = SceneManager.GetSceneByName(sceneReference.Name);
            if (!scene.isLoaded) return;
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = autoSwitch;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }
        }

        /// <summary>
        /// Awaits Until sceneOperation ready to switch
        /// </summary>
        /// <param name="onProgressChanged"></param>
        /// <param name="sceneOperation"></param>
        /// <returns></returns>
        private static bool Until(SceneLoaderProgressChanged onProgressChanged, AsyncOperation sceneOperation)
        {
            onProgressChanged?.Invoke(sceneOperation.progress);
            return sceneOperation.allowSceneActivation ? sceneOperation.isDone : sceneOperation.progress >= 0.9f;
        }

        /// <summary>
        /// Loads SceneLoaderAsset
        /// </summary>
        /// <param name="sceneReference"></param>
        /// <param name="mode"></param>
        /// <param name="autoSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        internal static async Task<AsyncOperation> SceneLoadOperation(this SceneReference sceneReference,
            LoadSceneMode mode, bool autoSwitch,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var sceneOperation = SceneManager.LoadSceneAsync(sceneReference.Name, mode);
            sceneOperation.allowSceneActivation = autoSwitch;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
        }

        public static Task LoadIntermediate(this SceneSystemSettings settings, LoadSceneMode sceneLoadMode, SceneLoaderProgressChanged progressChanged)
        {
            return settings.IntermediateScene.SceneLoadOperation(sceneLoadMode, true, progressChanged);
        }

        public static Task WaitForIntermediate(this SceneSystemSettings settings)
        {
            return Task.Delay(settings.TimeInIntermediateScene * Seconds);
        }

        public static Task UnloadIntermediate(this SceneSystemSettings settings, UnloadSceneOptions unloadSceneOptions, bool autoSwitch)
        {
            return settings.IntermediateScene.SceneUnloadOperation(unloadSceneOptions, autoSwitch);
        }

        public static bool Validate(this SceneReference reference)
        {
            return reference != null && !string.IsNullOrEmpty(reference.FullPath) && !string.IsNullOrEmpty(reference.Name) && !string.IsNullOrEmpty(reference.Guid);
        }
    }
}