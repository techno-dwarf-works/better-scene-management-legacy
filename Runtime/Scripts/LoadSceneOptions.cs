using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    public delegate void SceneLoaderProgressChanged(float value);
    public delegate void IntermediateLoaded();
    public delegate Task IntermediateLoadedAsync();

    /// <summary>
    /// Loading Scene Options
    /// </summary>
    [Serializable]
    public class LoadSceneOptions
    {
        public LoadSceneMode SceneLoadMode { get; set; } = LoadSceneMode.Additive;
        public bool UseIntermediate { get; set; } = true;
        public SceneLoaderProgressChanged ProgressChanged { get; set; } = null;
        public IntermediateLoaded IntermediateLoaded { get; set; } = null;
        public IntermediateLoadedAsync CustomAwaiter { get; set; } = null;
    }
    
    public class UnLoadSceneOptions
    {
        public UnloadSceneOptions SceneUnloadMode { get; set; } = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;

        public SceneLoaderProgressChanged ProgressChanged { get; set; } = null;
    }
}