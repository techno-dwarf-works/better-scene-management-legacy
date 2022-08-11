using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement.Runtime
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
        
        public static async Task WaitUntilDone(this AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
        }
        
        /// <summary>
        /// Unloads scene by SceneLoaderAsset
        /// </summary>
        /// <param name="sceneLoaderAsset"></param>
        /// <param name="mode"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneUnloadOperation(this SceneLoaderAsset sceneLoaderAsset, UnloadSceneOptions mode,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var scene = SceneManager.GetSceneByName(sceneLoaderAsset.Name);
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = false;
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
        public static bool Until(SceneLoaderProgressChanged onProgressChanged, AsyncOperation sceneOperation)
        {
            onProgressChanged?.Invoke(sceneOperation.progress);
            return sceneOperation.progress >= 0.9f;
        }

        /// <summary>
        /// Loads SceneLoaderAsset
        /// </summary>
        /// <param name="sceneAsset"></param>
        /// <param name="mode"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneLoadOperation(this SceneLoaderAsset sceneAsset,
            LoadSceneMode mode,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var sceneOperation = SceneManager.LoadSceneAsync(sceneAsset.Name, mode);
            sceneOperation.allowSceneActivation = false;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
        }

        /// <summary>
        /// Unloads current Scene
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static async Task<AsyncOperation> SceneUnloadOperation(this string name, UnloadSceneOptions mode,
            SceneLoaderProgressChanged onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(name, mode);
            sceneOperation.allowSceneActivation = false;
            while (!Until(onProgressChanged, sceneOperation))
            {
                await Task.Yield();
            }

            return sceneOperation;
        }
    }
}