using System.Threading.Tasks;
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Transitions;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    public class InternalSceneSystem : ISystem
    {
        private readonly SceneSystemSettings _settings;

        public InternalSceneSystem()
        {
            _settings = SceneSystemSettings.Instance;
        }

        public SingleTransitionInfo CreateSingleTransition(SceneReference sceneReference)
        {
            return new(this, sceneReference);
        }

        public AdditiveTransitionInfo CreateAdditiveTransition()
        {
            return new(this);
        }

        Task ITransitionRunner<SingleTransitionInfo>.RunAsync(SingleTransitionInfo transitionInfo)
        {
            return RunAsync(transitionInfo, LoadSceneMode.Single);
        }

        Task ITransitionRunner<AdditiveTransitionInfo>.RunAsync(AdditiveTransitionInfo transitionInfo)
        {
            return RunAsync(transitionInfo, LoadSceneMode.Additive);
        }

        private Task RunAsync(TransitionInfo transitionInfo, LoadSceneMode mode)
        {
            if (!transitionInfo.SequenceOverriden
                || !_settings.TryGetSequence(transitionInfo.SequenceType, out var sequence))
            {
                sequence = _settings.GetDefaultSequence();
            }

            var fromOperations = transitionInfo.CollectFromOperations();
            var toOperations = transitionInfo.CollectToOperations();
            return sequence.Run(fromOperations, toOperations, mode);
        }
    }
}