using Better.Tools.Runtime.Settings;
using UnityEngine;

namespace Better.SceneManagement.Runtime
{
    /// <summary>
    /// Scene Loader Settings
    /// </summary>
    public class SceneLoaderSettings : ProjectSettings
    {
        [SerializeField] private SceneLoaderAsset intermediateScene = new SceneLoaderAsset();

        [Tooltip("Time in seconds")] [Min(0)] [SerializeField]
        private int timeInIntermediateScene;

        public SceneLoaderAsset IntermediateScene => intermediateScene;

        public int TimeInIntermediateScene => timeInIntermediateScene;
    }
}