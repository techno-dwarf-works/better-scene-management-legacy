using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    /// <summary>
    /// Scene Loader at run-time
    /// </summary>
    public static class SceneLoader
    {
        private static readonly SceneLoaderSettings SceneLoaderSettings;

        static SceneLoader()
        {
            SceneLoaderSettings = Resources.Load<SceneLoaderSettings>(nameof(SceneLoaderSettings));
        }

        /// <summary>
        /// Unloads async SceneLoaderAsset with UnLoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="unloadSceneOptions"></param>
        public static async Task UnloadSceneAsync(SceneLoaderAsset asset, UnLoadSceneOptions unloadSceneOptions)
        {
            await UnloadSceneInternal(asset, unloadSceneOptions);
        }

        /// <summary>
        /// Unloads SceneLoaderAsset with UnLoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="unloadSceneOptions"></param>
        public static async void UnloadScene(SceneLoaderAsset asset, UnLoadSceneOptions unloadSceneOptions)
        {
            await UnloadSceneInternal(asset, unloadSceneOptions);
        }

        /// <summary>
        /// Unloads async SceneLoaderAsset with UnLoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async Task UnloadSceneAsync(SceneLoaderAsset asset)
        {
            await UnloadSceneInternal(asset, new UnLoadSceneOptions());
        }

        /// <summary>
        /// Unloads SceneLoaderAsset with UnLoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async void UnloadScene(SceneLoaderAsset asset)
        {
            await UnloadSceneInternal(asset, new UnLoadSceneOptions());
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with default LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async Task LoadSceneAsync(SceneLoaderAsset asset)
        {
            await LoadSceneInternal(asset, new LoadSceneOptions());
        }

        /// <summary>
        /// Loads SceneLoaderAsset with default LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async void LoadScene(SceneLoaderAsset asset)
        {
            await LoadSceneInternal(asset, new LoadSceneOptions());
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="options"></param>
        public static async Task LoadSceneAsync(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await LoadSceneInternal(asset, options);
        }

        /// <summary>
        /// Loads async SceneLoaderAsset with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="options"></param>
        public static async void LoadScene(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await LoadSceneInternal(asset, options);
        }

        /// <summary>
        /// Loads scene async with intermediate scene from <see cref="SceneLoaderSettings"/> with default LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async Task LoadSceneWithIntermediateAsync(SceneLoaderAsset asset)
        {
            await LoadSceneWithIntermediateInternal(asset, new LoadSceneOptions());
        }

        /// <summary>
        /// Loads scene async with intermediate scene from <see cref="SceneLoaderSettings"/> with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="options"></param>
        public static async Task LoadSceneWithIntermediateAsync(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await LoadSceneWithIntermediateInternal(asset, options);
        }
        
        /// <summary>
        /// Loads scene async with intermediate scene from <see cref="SceneLoaderSettings"/> with default LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        public static async void LoadSceneWithIntermediate(SceneLoaderAsset asset)
        {
            await LoadSceneWithIntermediateInternal(asset, new LoadSceneOptions());
        }

        /// <summary>
        /// Loads scene async with intermediate scene from <see cref="SceneLoaderSettings"/> with LoadSceneOptions
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="options"></param>
        public static async void LoadSceneWithIntermediate(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await LoadSceneWithIntermediateInternal(asset, options);
        }

        private static async Task LoadSceneWithIntermediateInternal(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await SceneLoaderSettings.LoadIntermediate(options.SceneLoadMode, options.ProgressChanged);

            options.IntermediateLoaded?.Invoke();
            if (options.CustomAwaiter != null)
                await options.CustomAwaiter.Invoke();

            var taskOperation = asset.SceneLoadOperation(options.SceneLoadMode, true, options.ProgressChanged);
            var intermediate = SceneLoaderSettings.WaitForIntermediate();

            await Task.WhenAll(taskOperation, intermediate);

            await SceneLoaderSettings.UnloadIntermediate(UnloadSceneOptions.UnloadAllEmbeddedSceneObjects, true);
        }

        private static async Task LoadSceneInternal(SceneLoaderAsset asset, LoadSceneOptions options)
        {
            await asset.SceneLoadOperation(options.SceneLoadMode, true, options.ProgressChanged);
        }

        private static async Task UnloadSceneInternal(SceneLoaderAsset asset, UnLoadSceneOptions unLoadSceneOptions)
        {
            await asset.SceneUnloadOperation(unLoadSceneOptions.SceneUnloadMode, true);
        }
    }
}