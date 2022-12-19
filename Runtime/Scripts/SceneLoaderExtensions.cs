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
        /// <summary>
        /// Unloads scene
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneUnloadOperation(this Scene scene, UnloadSceneOptions mode,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = false;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
        }

        /// <summary>
        /// Unloads scene by SceneLoaderAsset
        /// </summary>
        /// <param name="sceneLoaderAsset"></param>
        /// <param name="mode"></param>
        /// <param name="autoSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneUnloadOperation(this SceneLoaderAsset sceneLoaderAsset, UnloadSceneOptions mode, bool autoSwitch,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var scene = SceneManager.GetSceneByName(sceneLoaderAsset.Name);
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = autoSwitch;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
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
        /// <param name="sceneAsset"></param>
        /// <param name="mode"></param>
        /// <param name="autoSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneLoadOperation(this SceneLoaderAsset sceneAsset,
            LoadSceneMode mode, bool autoSwitch,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var sceneOperation = SceneManager.LoadSceneAsync(sceneAsset.Name, mode);
            sceneOperation.allowSceneActivation = autoSwitch;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
        }

        public static bool Validate(this SceneLoaderAsset asset)
        {
            return asset != null && !string.IsNullOrEmpty(asset.FullPath) && !string.IsNullOrEmpty(asset.Name) && !string.IsNullOrEmpty(asset.Guid);
        }
    }
}