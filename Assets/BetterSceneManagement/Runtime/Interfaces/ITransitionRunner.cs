using System.Threading.Tasks;
using Better.SceneManagement.Runtime.Transitions;

namespace Better.SceneManagement.Runtime.Interfaces
{
    public interface ITransitionRunner<in TInfo> where TInfo : TransitionInfo
    {
        public Task RunAsync(TInfo transitionInfo);
    }
}