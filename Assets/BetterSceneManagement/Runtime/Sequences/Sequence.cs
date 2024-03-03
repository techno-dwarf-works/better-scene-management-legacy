using System;
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

        public abstract Task Run(OperationData[] fromOperations, OperationData[] toOperations, LoadSceneMode mode);

        protected static async Task RunOperation(OperationData data, LoadSceneMode mode)
        {
            if (!data.Validate())
            {
                return;
            }

            var operation = SceneManager.LoadSceneAsync(data.SceneReference.Name, mode);
            operation.allowSceneActivation = true;
            await operation.AwaitCompletion(data.ProgressCallback);
        }
    }
}