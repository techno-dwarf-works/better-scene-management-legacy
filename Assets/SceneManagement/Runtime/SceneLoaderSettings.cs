using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement.Runtime
{
    /// <summary>
    /// Scene Loader Settings
    /// </summary>
    public class SceneLoaderSettings : ScriptableObject
    {
        [SerializeField] private List<SceneLoaderAsset> scenes;
        [SerializeField] private SceneLoaderAsset intermediateScene;
        [Tooltip("Time in seconds")]
        [Min(0)] [SerializeField] private int timeInIntermediateScene;

        public List<SceneLoaderAsset> Scenes => scenes;

        public SceneLoaderAsset IntermediateScene => intermediateScene;

        public int TimeInIntermediateScene => timeInIntermediateScene;

        public void ResetSceneList(List<SceneLoaderAsset> sceneLoaderAssets)
        {
            scenes = sceneLoaderAssets;
        }
    }
}
