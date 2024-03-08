using System;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime.Sequences
{
    [Serializable]
    public class InverseGraduallySequence : Sequence
    {
        public override async Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode, bool logs)
        {
            await Load(loadOperations, mode, logs);

            if (!unloadOperations.IsEmpty() && ValidateSceneMode(mode, LoadSceneMode.Additive, logs))
            {
                await Unload(unloadOperations, logs);
            }
        }
    }
}