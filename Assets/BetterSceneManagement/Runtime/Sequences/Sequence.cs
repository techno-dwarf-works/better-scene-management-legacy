using System;
using System.Linq;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using UnityEngine.SceneManagement;

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
                var isValid = SceneReference.Validate();
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

            var operation = SceneManager.LoadSceneAsync(data.SceneReference.Name, mode);
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

            var operation = SceneManager.UnloadSceneAsync(data.SceneReference.Name);
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
    }
}