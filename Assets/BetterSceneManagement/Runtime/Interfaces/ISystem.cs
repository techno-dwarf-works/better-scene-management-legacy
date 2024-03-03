using Better.SceneManagement.Runtime.Transitions;

namespace Better.SceneManagement.Runtime.Interfaces
{
    public interface ISystem : ITransitionRunner<SingleTransitionInfo>, ITransitionRunner<AdditiveTransitionInfo>
    {
    }
}