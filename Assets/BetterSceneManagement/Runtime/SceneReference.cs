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
        [SerializeField, HideInInspector] private string _name;
        [SerializeField, HideInInspector] private string _fullPath;
        [SerializeField, HideInInspector] private string _guid;

        public string FullPath => _fullPath;
        public string Name => _name;
        public string Guid => _guid;

        public SceneReference(string path, string guid)
        {
            _fullPath = path;
            _guid = guid;
            _name = Path.GetFileNameWithoutExtension(_fullPath);
        }

        public SceneReference(Scene scene)
        {
            _fullPath = scene.path;
            _guid = "1";
            _name = Path.GetFileNameWithoutExtension(_fullPath);
        }

        public SceneReference()
        {
            _fullPath = string.Empty;
            _guid = string.Empty;
            _name = string.Empty;
        }

        public bool Validate()
        {
            return !_fullPath.IsNullOrEmpty() && !_name.IsNullOrEmpty() && !_guid.IsNullOrEmpty();
        }

        public bool Equals(SceneReference obj)
        {
            return _name.CompareOrdinal(obj._name) && _fullPath.CompareOrdinal(obj._fullPath);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}