#if BETTER_SERVICES
using System.Threading;
using System.Threading.Tasks;
using Better.SceneManagement.Runtime.Interfaces;
using Better.SceneManagement.Runtime.Transitions;
using Better.Services.Runtime;

namespace Better.SceneManagement.Runtime
{
    public class SavesService : PocoService, ISceneSystem
    {
        private ISceneSystem _internalSystem;

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            _internalSystem = new InternalSceneSystem();
            return Task.CompletedTask;
        }

        protected override Task OnPostInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region ISceneSystem

        public SingleTransitionInfo CreateSingleTransition(SceneReference sceneReference) => _internalSystem.CreateSingleTransition(sceneReference);
        public AdditiveTransitionInfo CreateAdditiveTransition() => _internalSystem.CreateAdditiveTransition();

        #endregion
    }
}
#endif