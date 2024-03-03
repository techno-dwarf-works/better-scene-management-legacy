using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Sequences;

namespace Better.SceneManagement.Runtime.Transitions
{
    public class AdditiveTransitionInfo : TransitionInfo
    {
        private readonly ITransitionRunner<AdditiveTransitionInfo> _runner;
        private readonly HashSet<Sequence.OperationData> _loadOperations;
        private readonly HashSet<Sequence.OperationData> _unloadOperations;
        private readonly Dictionary<SceneReference, Sequence.OperationData> _sceneOperationMap;

        public AdditiveTransitionInfo(ITransitionRunner<AdditiveTransitionInfo> runner) : base()
        {
            _runner = runner;
            _loadOperations = new();
            _unloadOperations = new();
            _sceneOperationMap = new();
        }

        public AdditiveTransitionInfo Sequence<TSequence>()
            where TSequence : Sequence
        {
            OverrideSequence<TSequence>();
            return this;
        }

        public AdditiveTransitionInfo LoadScene(SceneReference sceneReference)
        {
            if (!ValidateMutable())
            {
                return this;
            }

            var operationData = new Sequence.OperationData(sceneReference);
            if (TryMappingOperation(operationData))
            {
                _loadOperations.Add(operationData);
            }

            return this;
        }

        public AdditiveTransitionInfo UnloadScene(SceneReference sceneReference)
        {
            if (!ValidateMutable())
            {
                return this;
            }

            var operationData = new Sequence.OperationData(sceneReference);
            if (TryMappingOperation(operationData))
            {
                _unloadOperations.Add(operationData);
            }

            return this;
        }

        public AdditiveTransitionInfo OnProgress(SceneReference sceneReference, EventHandler<float> callback)
        {
            if (!ValidateMutable())
            {
                return this;
            }

            if (_sceneOperationMap.TryGetValue(sceneReference, out var operationData))
            {
                operationData.ProgressCallback.ProgressChanged += callback;
            }
            else
            {
                var message = $"{nameof(sceneReference)}({sceneReference}) not mapped, {nameof(callback)} not registred";
                DebugUtility.LogException<InvalidOperationException>(message);
            }

            return this;
        }

        public override Sequence.OperationData[] CollectUnloadOperations()
        {
            return _unloadOperations.ToArray();
        }

        public override Sequence.OperationData[] CollectLoadOperations()
        {
            return _loadOperations.ToArray();
        }

        public Task RunAsync()
        {
            if (!ValidateMutable())
            {
                return Task.CompletedTask;
            }

            MakeImmutable();
            return _runner.RunAsync(this);
        }

        public void Run()
        {
            RunAsync().Forget();
        }

        private bool TryMappingOperation(Sequence.OperationData value, bool logException = true)
        {
            if (_sceneOperationMap.ContainsKey(value.SceneReference))
            {
                if (logException)
                {
                    var message = $"{nameof(value.SceneReference)}({value.SceneReference}) already mapped";
                    DebugUtility.LogException<InvalidOperationException>(message);
                }

                return false;
            }

            _sceneOperationMap.Add(value.SceneReference, value);
            return true;
        }
    }
}