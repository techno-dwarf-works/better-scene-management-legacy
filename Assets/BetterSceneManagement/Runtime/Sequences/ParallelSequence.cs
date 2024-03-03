using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime.Sequences
{
    [Serializable]
    public class ParallelSequence : Sequence
    {
        public override Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode)
        {
            var tasks = new List<Task>();
            if (!unloadOperations.IsEmpty() && ValidateSceneMode(mode, LoadSceneMode.Additive))
            {
                var unloadTask = Unload(unloadOperations);
                tasks.Add(unloadTask);
            }

            var loadTask = Load(loadOperations, mode);
            tasks.Add(loadTask);
            
            return tasks.WhenAll();
        }
    }
}