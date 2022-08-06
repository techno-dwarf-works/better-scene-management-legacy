using System;
using UnityEngine.SceneManagement;

namespace SceneManagement.Runtime
{
    /// <summary>
    /// Loading Scene Options
    /// </summary>
    [Serializable]
    public class LoadSceneOptions
    {
        public LoadSceneMode SceneLoadMode { get; set; } = LoadSceneMode.Additive;
        public UnloadSceneOptions SceneUnloadMode { get; set; } = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
        public bool UseIntermediate { get; set; } = true;
    }
}
