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
        /// <param name="loadSceneOptions"></param>
        public static async Task LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions loadSceneOptions)
        {
            switch (loadSceneOptions.UseIntermediate)
            {
                case true:
                    await LoadSceneWithIntermediate(asset, loadSceneOptions);
                    break;
                case false:
                    await LoadScene(asset, loadSceneOptions);
                    break;
            }
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with default options
        /// </summary>
        /// <param name="asset"></param>
        public static async Task LoadSceneAsync(SceneLoaderAsset asset)
        {
            await LoadSceneAsync(asset, new LoadSceneOptions());
        }

        private static async Task LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var currentScene = SceneManager.GetActiveScene();

            var sceneAsyncOperation =
                await SceneLoaderSettings.IntermediateScene.SceneLoadOperation(options.SceneLoadMode,
                    options.ProgressChanged);
            sceneAsyncOperation.allowSceneActivation = true;

            await sceneAsyncOperation.WaitUntilDone();

            var unloadCurrentOperation = await currentScene.SceneUnloadOperation(options.SceneUnloadMode);
            unloadCurrentOperation.allowSceneActivation = true;

            var nextSceneOperation = await asset.SceneLoadOperation(options.SceneLoadMode, options.ProgressChanged);
            await Task.Delay(SceneLoaderSettings.TimeInIntermediateScene * Seconds);
            nextSceneOperation.allowSceneActivation = true;

            if (options.SceneLoadMode == LoadSceneMode.Single) return;

            await nextSceneOperation.WaitUntilDone();

            var unloadIntermediate =
                await SceneLoaderSettings.IntermediateScene.SceneUnloadOperation(options.SceneUnloadMode);
            unloadIntermediate.allowSceneActivation = true;

            await unloadIntermediate.WaitUntilDone();
        }

        private static async Task LoadScene(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            var currentScene = SceneManager.GetActiveScene();
            var nextSceneOperation =
                await asset.SceneLoadOperation(options.SceneLoadMode, options.ProgressChanged);
            nextSceneOperation.allowSceneActivation = true;

            await nextSceneOperation.WaitUntilDone();

            if (options.SceneLoadMode == LoadSceneMode.Single) return;

            var operation = await currentScene.SceneUnloadOperation(options.SceneUnloadMode);
            operation.allowSceneActivation = true;

            await operation.WaitUntilDone();
        }
    }
}