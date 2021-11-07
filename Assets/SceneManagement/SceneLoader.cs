using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    /// <summary>
    /// Scene Loader at run-time
    /// </summary>
    public static class SceneLoader
    {
        #if SYMBOL_DEFINER_ASSET
        [SymbolDefiner(true)]
        #endif
        private const string SceneManagementAsset = "SCENE_MANAGMENT_ASSET";
        private static readonly SceneLoaderSettings SceneLoaderSettings;

        static SceneLoader()
        {
            SceneLoaderSettings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="loadSceneOptions"></param>
        /// <param name="monoBehaviour">MonoBehaviour to load on, use objects marked as DoNotDestroyOnLoad</param>
        public static void LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions loadSceneOptions,
                                          MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour == null)
            {
                Debug.LogException(new ArgumentException($"{nameof(monoBehaviour)} should be not null"));
                return;
            }

            switch (loadSceneOptions.UseIntermediate)
            {
                case true:
                    monoBehaviour.StartCoroutine(LoadSceneWithIntermediate(asset, loadSceneOptions, monoBehaviour));
                    break;
                case false:
                    monoBehaviour.StartCoroutine(LoadScene(asset, loadSceneOptions));
                    break;
            }
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with default options
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="monoBehaviour">MonoBehaviour to load on, use objects marked as DoNotDestroyOnLoad</param>
        public static void LoadSceneAsync(SceneLoaderAsset asset, MonoBehaviour monoBehaviour)
        {
            LoadSceneAsync(asset, new LoadSceneOptions(), monoBehaviour);
        }

        private static IEnumerator LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options,
                                                             MonoBehaviour monoBehaviour)
        {
            var currentScene = SceneManager.GetActiveScene();

            var intermediateCoroutine =
                monoBehaviour.StartCoroutine(SceneLoaderSettings.IntermediateScene
                                                                .SceneLoadOperation(options.SceneLoadMode,
                                                                     sceneAsyncOperation =>
                                                                         sceneAsyncOperation.allowSceneActivation =
                                                                             true));
            AsyncOperation nextSceneOperation = null;

            var sceneOperationCoroutine =
                monoBehaviour.StartCoroutine(asset.SceneLoadOperation(options.SceneLoadMode,
                                                                      sceneAsyncOperation =>
                                                                          nextSceneOperation = sceneAsyncOperation));
            yield return new WaitForSeconds(SceneLoaderSettings.TimeInIntermediateScene);
            yield return intermediateCoroutine;
            yield return sceneOperationCoroutine;
            nextSceneOperation.allowSceneActivation = true;
            if (options.SceneLoadMode == LoadSceneMode.Single) yield break;
            yield return new WaitUntil(() => nextSceneOperation.isDone);

            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode,
                                                           operation => { operation.allowSceneActivation = true; });
        }

        private static IEnumerator LoadScene(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var currentScene = SceneManager.GetActiveScene();
            AsyncOperation nextSceneOperation = null;

            yield return asset.SceneLoadOperation(options.SceneLoadMode,
                                                  sceneOperation => nextSceneOperation = sceneOperation);
            nextSceneOperation.allowSceneActivation = true;
            if (options.SceneLoadMode == LoadSceneMode.Single) yield break;
            yield return new WaitUntil(() => nextSceneOperation.isDone);

            yield return currentScene.SceneUnloadOperation(options.SceneUnloadMode,
                                                           operation => { operation.allowSceneActivation = true; });
        }
    }
}
