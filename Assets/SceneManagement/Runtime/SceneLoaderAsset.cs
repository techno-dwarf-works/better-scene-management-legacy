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
        [SerializeField] private int instanceID;

        public string FullPath => fullPath;

        public string Name => name;

        public int InstanceID => instanceID;

        public SceneLoaderAsset(string path, int instanceID)
        {
            fullPath = path;
            this.instanceID = instanceID;
            name = Path.GetFileNameWithoutExtension(path);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(fullPath) && !string.IsNullOrEmpty(name) && instanceID != 0;
        }
    }
}
