using System.Collections.Generic;
using UnityEditor;

namespace SceneManagement.Editor
{
    internal class EditorSceneComparer : IEqualityComparer<EditorBuildSettingsScene>
    {
        public bool Equals(EditorBuildSettingsScene s1, EditorBuildSettingsScene s2)
        {
            return s2 == null && s1 == null || s1 != null && s2 != null && s1.path == s2.path;
        }

        public int GetHashCode(EditorBuildSettingsScene scene)
        {
            var hCode = scene.path;
            return hCode.GetHashCode();
        }
    }
}
