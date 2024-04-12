﻿using System;
using System.Threading.Tasks;
using Better.Commons.Runtime.Extensions;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime.Sequences
{
    [Serializable]
    public class GraduallySequence : Sequence
    {
        public override async Task Run(OperationData[] unloadOperations, OperationData[] loadOperations, LoadSceneMode mode, bool logs)
        {
            if (!unloadOperations.IsEmpty() && ValidateSceneMode(mode, LoadSceneMode.Additive, logs))
            {
                await Unload(unloadOperations, logs);
            }

            await Load(loadOperations, mode, logs);
        }
    }
}