using System;
using System.Linq;
using System.Threading.Tasks;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Better.SceneManagement.Runtime.Sequences
{
    [Serializable]
    public abstract class Sequence
    {
        public class OperationData
        {
            public SceneReference SceneReference { get; }
            public Progress<float> ProgressCallback { get; }

            public OperationData(SceneReference sceneReference)
            {
                SceneReference = sceneReference;
                ProgressCallback = new();
            }

            public bool Validate(bool logException = true)
            {
                var isValid = SceneReference != null && SceneReference.Validate();
                if (!isValid && logException)
                {
                    var message = $"{nameof(SceneReference)} is invalid";
                    DebugUtility.LogException<InvalidOperationException>(message);
                }

                return isValid;
            }
        }

        public abstract Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode, bool logs);

        protected async Task Load(OperationData data, LoadSceneMode mode, bool logs)
        {
            if (!data.Validate())
            {
                return;
            }

            var sceneName = data.SceneReference.Name;
            if (!ValidateSceneLoaded(sceneName, false, logs))
            {
                return;
            }

            var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
            operation.allowSceneActivation = true;
            await operation.AwaitCompletion(data.ProgressCallback);
        }

        protected Task Load(OperationData[] data, LoadSceneMode mode, bool logs)
        {
            return data.Select(element => Load(element, mode, logs)).WhenAll();
        }

        protected async Task Unload(OperationData data, bool logs)
        {
            if (!data.Validate())
            {
                return;
            }

            var sceneName = data.SceneReference.Name;
            if (!ValidateSceneLoaded(sceneName, true, logs)
                || !ValidateSubScene(sceneName, true, logs))
            {
                return;
            }

            var operation = UnitySceneManager.UnloadSceneAsync(sceneName);
            await operation.AwaitCompletion(data.ProgressCallback);
        }

        protected Task Unload(OperationData[] data, bool logs)
        {
            return data.Select(element => Unload(element, logs)).WhenAll();
        }

        protected bool ValidateSceneMode(LoadSceneMode value, LoadSceneMode validState, bool logException)
        {
            var isValid = value == validState;
            if (!isValid && logException)
            {
                var message = $"{nameof(value)}({value}) is not {nameof(validState)}({validState})";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }

        protected bool ValidateSceneLoaded(string sceneName, bool isLoaded, bool logException)
        {
            var scene = UnitySceneManager.GetSceneByName(sceneName);
            var isValid = isLoaded == (scene.IsValid() && scene.isLoaded);
            if (!isValid && logException)
            {
                var reason = isLoaded ? "not loaded" : "already loaded";
                var message = $"{nameof(scene)}({sceneName}) not valid, {reason}";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }

        protected bool ValidateSubScene(string sceneName, bool isSubScene, bool logException)
        {
            var activeScene = UnitySceneManager.GetActiveScene();
            var isActive = activeScene.IsValid() && activeScene.name == sceneName;
            var isValid = isActive != isSubScene;
            if (!isValid && logException)
            {
                var reason = isSubScene ? "not subScene" : "is activeScene";
                var message = $"{sceneName} not valid, {reason}";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}