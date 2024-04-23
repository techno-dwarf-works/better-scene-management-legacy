#if BETTER_SINGLETONS
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Transitions;
using Better.Singletons.Runtime;

namespace Better.SceneManagement.Runtime
{
    public class SceneManager : PocoSingleton<SceneManager>, ISceneSystem
    {
        private readonly ISceneSystem _internalSystem;

        public SceneManager()
        {
            _internalSystem = new InternalSceneSystem();
        }

        #region ISceneSystem

        public SingleTransitionInfo CreateSingleTransition(SceneReference sceneReference) => _internalSystem.CreateSingleTransition(sceneReference);
        public AdditiveTransitionInfo CreateAdditiveTransition() => _internalSystem.CreateAdditiveTransition();

        #endregion
    }
}
#endif