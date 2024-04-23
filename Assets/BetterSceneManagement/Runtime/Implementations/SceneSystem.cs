using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Transitions;

namespace Better.SceneManagement.Runtime
{
    public class SceneSystem : ISceneSystem
    {
        private static ISceneSystem _internalSystem;

        public SceneSystem()
        {
            if (_internalSystem == null)
            {
                _internalSystem = new InternalSceneSystem();
            }
        }

        #region ISceneSystem

        public SingleTransitionInfo CreateSingleTransition(SceneReference sceneReference) => _internalSystem.CreateSingleTransition(sceneReference);
        public AdditiveTransitionInfo CreateAdditiveTransition() => _internalSystem.CreateAdditiveTransition();

        #endregion
    }
}