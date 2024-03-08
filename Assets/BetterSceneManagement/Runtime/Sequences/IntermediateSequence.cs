using System;
using System.Linq;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime.Sequences
{
    [Serializable]
    public class IntermediateSequence : Sequence
    {
        [SerializeField] private SceneReference _intermediateScene;

        [Min(0f)]
        [SerializeField] private float _duration;

        public override async Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode, bool logs)
        {
            if (!ValidateRunOperations(unloadOperations, true, logs)
                || !ValidateRunOperations(loadOperations, false, logs))
            {
                return;
            }
            
            var intermediateOperation = new OperationData(_intermediateScene);
            await Load(intermediateOperation, mode, logs);

            if (!unloadOperations.IsEmpty() && ValidateSceneMode(mode, LoadSceneMode.Additive, logs))
            {
                await Unload(unloadOperations, logs);
            }

            await TaskUtility.WaitForSeconds(_duration);
            await Load(loadOperations, mode, logs);

            if (ValidateSceneMode(mode, LoadSceneMode.Additive, false))
            {
                await Unload(intermediateOperation, logs);
            }
        }

        private bool ValidateRunOperations(OperationData[] operations, bool isLoaded, bool logs)
        {
            if (operations.IsEmpty())
            {
                return true;
            }

            var isValid = operations.Select(o => o.SceneReference.Name)
                .All(sceneName => ValidateSceneLoaded(sceneName, isLoaded, logs));

            if (!isValid && logs)
            {
                var message = "Contains invalid operations, run cancelled";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return isValid;
        }
    }
}