using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    /// <summary>
    /// Scene Loader Settings
    /// </summary>
    public class SceneLoaderSettings : ScriptableObject
    {
        [SerializeField] private SceneLoaderAsset intermediateScene = new SceneLoaderAsset();

        [Tooltip("Time in seconds")] [Min(0)] [SerializeField]
        private int timeInIntermediateScene;

        public SceneLoaderAsset IntermediateScene => intermediateScene;
        
        private const int Seconds = 1000;

        internal async Task LoadIntermediate(LoadSceneMode sceneLoadMode, SceneLoaderProgressChanged progressChanged)
        {
            await intermediateScene.SceneLoadOperation(sceneLoadMode, true, progressChanged);
        }

        internal async Task WaitForIntermediate()
        {
            await Task.Delay(timeInIntermediateScene * Seconds);
        }

        internal async Task UnloadIntermediate(UnloadSceneOptions unloadSceneOptions, bool autoSwitch)
        {
            await intermediateScene.SceneUnloadOperation(unloadSceneOptions, autoSwitch);
        }
    }
}