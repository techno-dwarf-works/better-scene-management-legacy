using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    /// <summary>
    /// Scene Loader Settings
    /// </summary>
    public class SceneLoaderSettings : ScriptableObject
    {
        [SerializeField] private List<SceneLoaderAsset> scenes;
        [SerializeField] private SceneLoaderAsset intermediateScene;
        [Min(0)] [SerializeField] private float timeInIntermediateScene;

        public List<SceneLoaderAsset> Scenes => scenes;

        public SceneLoaderAsset IntermediateScene => intermediateScene;

        public float TimeInIntermediateScene => timeInIntermediateScene;

        public void ResetSceneList(List<SceneLoaderAsset> sceneLoaderAssets)
        {
            scenes = sceneLoaderAssets;
        }
    }
}
