using System;
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


        public override async Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode)
        {
            var intermediateOperation = new OperationData(_intermediateScene);
            await Load(intermediateOperation, mode);

            if (!unloadOperations.IsEmpty() && ValidateSceneMode(mode, LoadSceneMode.Additive))
            {
                await Unload(unloadOperations);
            }

            await TaskUtility.WaitForSeconds(_duration);
            await Load(loadOperations, mode);

            if (ValidateSceneMode(mode, LoadSceneMode.Additive, false))
            {
                await Unload(intermediateOperation);
            }
        }
    }
}