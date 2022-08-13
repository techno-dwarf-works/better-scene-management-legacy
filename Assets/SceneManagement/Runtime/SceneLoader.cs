using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement.Runtime
{
    /// <summary>
    /// Scene Loader at run-time
    /// </summary>
    public static class SceneLoader
    {
        private const string SceneManagementAsset = "SCENE_MANAGEMENT_ASSET";
        private const int Seconds = 1000;
        private static readonly SceneLoaderSettings SceneLoaderSettings;

        static SceneLoader()
        {
            SceneLoaderSettings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="unloadSceneOptions"></param>
        public static async Task UnLoadSceneAsync(SceneLoaderAsset asset, UnLoadSceneOptions unloadSceneOptions)
        {
            await UnLoadScene(asset, unloadSceneOptions);
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async Task UnLoadSceneAsync(SceneLoaderAsset asset)
        {
            await UnLoadScene(asset, new UnLoadSceneOptions());
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with default options
        /// </summary>
        /// <param name="asset"></param>
        public static async Task LoadSceneAsync(SceneLoaderAsset asset)
        {
            await LoadSceneAsync(asset, new LoadSceneOptions());
        }

        private static async Task WaitUntil(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
        }

        public static async Task LoadSceneWithIntermediate(SceneLoaderAsset asset, Func<Task> intermediateLoaded = null)
        {
            await LoadSceneWithIntermediate(asset, new LoadSceneOptions(), intermediateLoaded);
        }

        public static async Task LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options,
            Func<Task> intermediateLoaded = null)
        {
            var intermediateOperation =
                await SceneLoaderSettings.IntermediateScene.SceneLoadOperation(options.SceneLoadMode,
                    options.ProgressChanged);
            intermediateOperation.allowSceneActivation = true;

            await WaitUntil(intermediateOperation);
            
            if (intermediateLoaded != null)
                await intermediateLoaded.Invoke();
            
            var nextSceneOperation = await asset.SceneLoadOperation(options.SceneLoadMode, options.ProgressChanged);
            await Task.Delay(SceneLoaderSettings.TimeInIntermediateScene * Seconds);
            nextSceneOperation.allowSceneActivation = true;

            await WaitUntil(nextSceneOperation);

            var unloadIntermediate =
                await SceneLoaderSettings.IntermediateScene.SceneUnloadOperation(UnloadSceneOptions
                    .UnloadAllEmbeddedSceneObjects);
            unloadIntermediate.allowSceneActivation = true;

            await WaitUntil(unloadIntermediate);
        }

        private static async Task UnLoadScene(SceneLoaderAsset sceneLoaderAsset, UnLoadSceneOptions unLoadSceneOptions)
        {
            await sceneLoaderAsset.SceneUnloadOperation(unLoadSceneOptions.SceneUnloadMode);
        }

        public static async Task LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var nextSceneOperation =
                await asset.SceneLoadOperation(options.SceneLoadMode, options.ProgressChanged);
            nextSceneOperation.allowSceneActivation = true;

            await WaitUntil(nextSceneOperation);
        }
    }
}