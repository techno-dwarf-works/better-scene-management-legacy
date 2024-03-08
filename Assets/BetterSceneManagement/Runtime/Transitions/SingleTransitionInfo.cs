using System;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Sequences;

namespace Better.SceneManagement.Runtime.Transitions
{
    public class SingleTransitionInfo : TransitionInfo
    {
        private readonly ITransitionRunner<SingleTransitionInfo> _runner;
        private readonly Sequence.OperationData _operationData;

        public SingleTransitionInfo(ITransitionRunner<SingleTransitionInfo> runner, SceneReference sceneReference, bool allowLogs)
            : base(allowLogs)
        {
            _runner = runner;
            _operationData = new(sceneReference);

            if (sceneReference == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(sceneReference));
                MakeImmutable();
            }
        }

        public SingleTransitionInfo Sequence<TSequence>()
            where TSequence : Sequence
        {
            OverrideSequence<TSequence>();
            return this;
        }

        public SingleTransitionInfo SuppressLogs()
        {
            OverrideAllowingLogs(false);
            return this;
        }

        public SingleTransitionInfo OnProgress(EventHandler<float> callback)
        {
            if (ValidateMutable() && callback != null)
            {
                _operationData.ProgressCallback.ProgressChanged += callback;
            }

            return this;
        }

        public override Sequence.OperationData[] CollectUnloadOperations()
        {
            return Array.Empty<Sequence.OperationData>();
        }

        public override Sequence.OperationData[] CollectLoadOperations()
        {
            return new[] { _operationData };
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
    }
}