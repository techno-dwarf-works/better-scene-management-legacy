using System;
using System.Linq;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
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

        public abstract Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode);

        protected async Task Load(OperationData data, LoadSceneMode mode)
        {
            if (!data.Validate())
            {
                return;
            }

            var sceneName = data.SceneReference.Name;
            if (!ValidateSceneLoaded(sceneName, false, false))
            {
                return;
            }

            var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
            operation.allowSceneActivation = true;
            await operation.AwaitCompletion(data.ProgressCallback);
        }

        protected Task Load(OperationData[] data, LoadSceneMode mode)
        {
            return data.Select(d => Load(d, mode)).WhenAll();
        }

        protected async Task Unload(OperationData data)
        {
            if (!data.Validate())
            {
                return;
            }

            var sceneName = data.SceneReference.Name;
            if (!ValidateSceneLoaded(sceneName, true, false)
                || !ValidateSubScene(sceneName, true, false))
            {
                return;
            }

            var operation = UnitySceneManager.UnloadSceneAsync(sceneName);
            await operation.AwaitCompletion(data.ProgressCallback);
        }

        protected Task Unload(OperationData[] data)
        {
            return data.Select(Unload).WhenAll();
        }

        protected bool ValidateSceneMode(LoadSceneMode value, LoadSceneMode validState, bool logException = true)
        {
            var isValid = value == validState;
            if (!isValid && logException)
            {
                var message = $"{nameof(value)}({value}) is not {nameof(validState)}({validState})";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }

        private bool ValidateSceneLoaded(string sceneName, bool isLoaded, bool logException = true)
        {
            var scene = UnitySceneManager.GetSceneByName(sceneName);
            var isValid = scene.IsValid() && scene.isLoaded == isLoaded;
            if (!isValid && logException)
            {
                var message = $"{nameof(scene)}({sceneName}) not valid ({nameof(isLoaded)}:{isLoaded})";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }

        private bool ValidateSubScene(string sceneName, bool isSubScene, bool logException = true)
        {
            var scene = UnitySceneManager.GetSceneByName(sceneName);
            var isValid = scene.IsValid() && scene.isSubScene == isSubScene;
            if (!isValid && logException)
            {
                var message = $"{nameof(scene)}({sceneName}) not valid ({nameof(isSubScene)}:{isSubScene})";
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