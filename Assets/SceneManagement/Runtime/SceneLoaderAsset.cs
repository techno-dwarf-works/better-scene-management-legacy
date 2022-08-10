using System;
using System.IO;
using UnityEngine;

namespace SceneManagement.Runtime
{
    /// <summary>
    /// Class represents SceneAsset for <seealso cref="SceneLoader"/> 
    /// </summary>
    [Serializable]
    public class SceneLoaderAsset
    {
        [SerializeField] private string fullPath;
        [SerializeField] private string name;
        [SerializeField] private string guid;

        public string FullPath => fullPath;

        public string Name => name;

        public string Guid => guid;

        public SceneLoaderAsset(string path, string guid)
        {
            fullPath = path;
            this.guid = guid;
            name = Path.GetFileNameWithoutExtension(path);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(fullPath) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(guid);
        }
    }
}
