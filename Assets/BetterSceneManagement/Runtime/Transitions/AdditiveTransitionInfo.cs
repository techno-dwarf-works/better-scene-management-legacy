using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Sequences;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Better.SceneManagement.Runtime.Transitions
{
    public class AdditiveTransitionInfo : TransitionInfo
    {
        private readonly ITransitionRunner<AdditiveTransitionInfo> _runner;
        private readonly HashSet<Sequence.OperationData> _loadOperations;
        private readonly HashSet<Sequence.OperationData> _unloadOperations;
        private readonly Dictionary<SceneReference, Sequence.OperationData> _sceneOperationMap;

        public AdditiveTransitionInfo(ITransitionRunner<AdditiveTransitionInfo> runner, bool allowLogs)
            : base(allowLogs)
        {
            _runner = runner;
            _loadOperations = new();
            _unloadOperations = new();
            _sceneOperationMap = new(SceneReferenceComparer.Comparer);
        }

        public AdditiveTransitionInfo Sequence<TSequence>()
            where TSequence : Sequence
        {
            OverrideSequence<TSequence>();
            return this;
        }

        public AdditiveTransitionInfo SuppressLogs()
        {
            OverrideAllowingLogs(false);
            return this;
        }

        public AdditiveTransitionInfo LoadScene(SceneReference sceneReference)
        {
            if (sceneReference == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReference));
                return this;
            }

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

        public AdditiveTransitionInfo LoadScenes(IEnumerable<SceneReference> sceneReferences)
        {
            if (sceneReferences == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReferences));
                return this;
            }

            if (!ValidateMutable())
            {
                return this;
            }

            foreach (var sceneReference in sceneReferences)
            {
                LoadScene(sceneReference);
            }

            return this;
        }

        public AdditiveTransitionInfo UnloadScene(SceneReference sceneReference)
        {
            if (sceneReference == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReference));
                return this;
            }

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

        public AdditiveTransitionInfo UnloadScenes(IEnumerable<SceneReference> sceneReferences)
        {
            if (sceneReferences == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReferences));
                return this;
            }

            if (!ValidateMutable())
            {
                return this;
            }

            foreach (var sceneReference in sceneReferences)
            {
                UnloadScene(sceneReference);
            }

            return this;
        }

        public AdditiveTransitionInfo UnloadAllScenes()
        {
            if (!ValidateMutable())
            {
                return this;
            }

            var activeScene = UnitySceneManager.GetActiveScene();
            var sceneCount = UnitySceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = UnitySceneManager.GetSceneAt(i);
                if (scene.IsValid() && scene.isLoaded && scene != activeScene)
                {
                    var sceneReference = new SceneReference(scene);
                    if (!IsMappedKey(sceneReference))
                    {
                        UnloadScene(sceneReference);
                    }
                }
            }

            return this;
        }

        public AdditiveTransitionInfo OnProgress(SceneReference sceneReference, EventHandler<float> callback)
        {
            if (sceneReference == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReference));
                return this;
            }

            if (!ValidateMutable() || callback == null)
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

        private bool IsMappedKey(SceneReference value)
        {
            return _sceneOperationMap.ContainsKey(value);
        }

        private bool TryMappingOperation(Sequence.OperationData value, bool logException = true)
        {
            if (IsMappedKey(value.SceneReference))
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