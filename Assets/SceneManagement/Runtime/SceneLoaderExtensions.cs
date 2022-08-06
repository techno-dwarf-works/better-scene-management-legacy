using System;
using System.Collections;
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
        /// Unloads current Scene
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        /// <param name="onSceneReadyToSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static IEnumerator SceneUnloadOperation(this Scene scene, UnloadSceneOptions mode,
                                                       Action<AsyncOperation> onSceneReadyToSwitch,
                                                       Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(scene, mode);
            sceneOperation.allowSceneActivation = false;
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }

        /// <summary>
        /// Awaits Until sceneOperation ready to switch
        /// </summary>
        /// <param name="onProgressChanged"></param>
        /// <param name="sceneOperation"></param>
        /// <returns></returns>
        public static bool Until(Action<float> onProgressChanged, AsyncOperation sceneOperation)
        {
            onProgressChanged?.Invoke(sceneOperation.progress);
            return sceneOperation.progress >= 0.9f;
        }

        /// <summary>
        /// Loads SceneLoaderAsset
        /// </summary>
        /// <param name="sceneAsset"></param>
        /// <param name="mode"></param>
        /// <param name="onSceneReadyToSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static IEnumerator SceneLoadOperation(this SceneLoaderAsset sceneAsset, LoadSceneMode mode,
                                                     Action<AsyncOperation> onSceneReadyToSwitch,
                                                     Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.LoadSceneAsync(sceneAsset.Name, mode);
            sceneOperation.allowSceneActivation = false;
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }

        /// <summary>
        /// Unloads current Scene
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <param name="onSceneReadyToSwitch"></param>
        /// <param name="onProgressChanged"></param>
        /// <returns></returns>
        public static IEnumerator SceneUnloadOperation(this string name, UnloadSceneOptions mode,
                                                       Action<AsyncOperation> onSceneReadyToSwitch,
                                                       Action<float> onProgressChanged = null)
        {
            var sceneOperation = SceneManager.UnloadSceneAsync(name, mode);
            sceneOperation.allowSceneActivation = false;
            yield return new WaitUntil(() => Until(onProgressChanged, sceneOperation));
            onSceneReadyToSwitch?.Invoke(sceneOperation);
        }
    }
}
