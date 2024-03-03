using System;
using System.IO;
using Better.Extensions.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.SceneManagement.Runtime
{
    [Serializable]
    public partial class SceneReference
    {
        [SerializeField] private string name;
        [SerializeField] private string fullPath;
        [SerializeField] private string guid;

        public string FullPath => fullPath;

        public string Name => name;

        public string Guid => guid;

        public SceneReference(string path, string guid)
        {
            fullPath = path;
            this.guid = guid;
            name = Path.GetFileNameWithoutExtension(fullPath);
        }

        public SceneReference(Scene scene)
        {
            fullPath = scene.path;
            guid = "1";
            name = Path.GetFileNameWithoutExtension(fullPath);
        }

        public SceneReference()
        {
            fullPath = string.Empty;
            guid = string.Empty;
            name = string.Empty;
        }

        public bool Equals(SceneReference obj)
        {
            return name.CompareOrdinal(obj.name) && fullPath.CompareOrdinal(obj.fullPath);
        }

        public override string ToString()
        {
            return name;
        }
    }
}