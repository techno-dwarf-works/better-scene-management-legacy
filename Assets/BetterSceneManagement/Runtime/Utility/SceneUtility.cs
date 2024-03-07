using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime.Interfaces;

namespace Better.SceneManagement.Runtime.Utility
{
    public static class SceneUtility
    {
#if BETTER_SERVICES && BETTER_LOCATOR
        private static readonly Locators.Runtime.ServiceProperty<SavesService> _serviceProperty = new();
#endif

        public static ISceneSystem GetSystem()
        {
#if BETTER_SERVICES && BETTER_LOCATOR
            if (_serviceProperty.IsRegistered)
            {
                return _serviceProperty.CachedService;
            }
#endif

#if BETTER_SINGLETONS
            return SceneManager.Instance;
#endif

#pragma warning disable CS0162
            return new SceneSystem();
#pragma warning restore CS0162
        }

        public static Task LoadSingleSceneAsync(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            return GetSystem()
                .CreateSingleTransition(sceneReference)
                .OnProgress(progressCallback)
                .RunAsync();
        }

        public static void LoadSingleScene(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            LoadSingleSceneAsync(sceneReference, progressCallback).Forget();
        }

        public static Task LoadAdditiveSceneAsync(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .LoadScene(sceneReference)
                .OnProgress(sceneReference, progressCallback)
                .RunAsync();
        }

        public static void LoadAdditiveScene(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            LoadAdditiveSceneAsync(sceneReference, progressCallback).Forget();
        }

        public static Task LoadAdditiveScenesAsync(IEnumerable<SceneReference> sceneReferences)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .LoadScenes(sceneReferences)
                .RunAsync();
        }

        public static void LoadAdditiveScenes(IEnumerable<SceneReference> sceneReferences)
        {
            LoadAdditiveScenesAsync(sceneReferences).Forget();
        }

        public static Task UnloadAdditiveSceneAsync(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .UnloadScene(sceneReference)
                .OnProgress(sceneReference, progressCallback)
                .RunAsync();
        }

        public static void UnloadAdditiveScene(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            UnloadAdditiveSceneAsync(sceneReference, progressCallback).Forget();
        }

        public static Task UnloadAdditiveScenesAsync(IEnumerable<SceneReference> sceneReferences)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .UnloadScenes(sceneReferences)
                .RunAsync();
        }

        public static void UnloadAdditiveScenes(IEnumerable<SceneReference> sceneReferences)
        {
            UnloadAdditiveScenesAsync(sceneReferences).Forget();
        }

        public static Task UnloadAllAdditiveScenesAsync()
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .UnloadAllScenes()
                .RunAsync();
        }

        public static void UnloadAllAdditiveScenes()
        {
            UnloadAllAdditiveScenesAsync().Forget();
        }

        public static Task SwapAdditiveSceneAsync(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .LoadScene(sceneReference)
                .OnProgress(sceneReference, progressCallback)
                .UnloadAllScenes()
                .RunAsync();
        }

        public static void SwapAdditiveScene(SceneReference sceneReference, EventHandler<float> progressCallback = null)
        {
            SwapAdditiveSceneAsync(sceneReference, progressCallback).Forget();
        }

        public static Task SwapAdditiveScenesAsync(IEnumerable<SceneReference> sceneReferences)
        {
            return GetSystem()
                .CreateAdditiveTransition()
                .LoadScenes(sceneReferences)
                .UnloadAllScenes()
                .RunAsync();
        }

        public static void SwapAdditiveScenes(IEnumerable<SceneReference> sceneReferences)
        {
            SwapAdditiveScenesAsync(sceneReferences).Forget();
        }
    }
}