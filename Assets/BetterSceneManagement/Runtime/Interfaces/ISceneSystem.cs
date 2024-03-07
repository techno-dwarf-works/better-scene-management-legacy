using Better.SceneManagement.Runtime.Transitions;

namespace Better.SceneManagement.Runtime.Interfaces
{
    public interface ISceneSystem
    {
        SingleTransitionInfo CreateSingleTransition(SceneReference sceneReference);
        AdditiveTransitionInfo CreateAdditiveTransition();
    }
}