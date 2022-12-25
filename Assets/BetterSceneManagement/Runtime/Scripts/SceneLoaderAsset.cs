using System;
using System.IO;
using Better.Extensions.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    /// <summary>
    /// Class represents SceneAsset for <seealso cref="SceneLoader"/> 
    /// </summary>
    [Serializable]
    public partial class SceneLoaderAsset
    {
        [SerializeField] private string name;
        [SerializeField] private string fullPath;
        [SerializeField] private string guid;

        public string FullPath => fullPath;

        public string Name => name;

        public string Guid => guid;

        public SceneLoaderAsset(string path, string guid)
        {
            fullPath = path;
            this.guid = guid;
            name = Path.GetFileNameWithoutExtension(fullPath);
        }

        public SceneLoaderAsset(Scene scene)
        {
            fullPath = scene.path;
            guid = "1";
            name = Path.GetFileNameWithoutExtension(fullPath);
        }
        
        public SceneLoaderAsset()
        {
            fullPath = string.Empty;
            guid = string.Empty;
            name = string.Empty;
        }

        public bool Equals(SceneLoaderAsset obj)
        {
            return name.FastEquals(obj.name) && fullPath.FastEquals(obj.fullPath);
        }
    }
}