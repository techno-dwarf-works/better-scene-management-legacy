using System.Collections.Generic;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
{
    internal class EditorSceneComparer : IEqualityComparer<EditorBuildSettingsScene>
    {
        public bool Equals(EditorBuildSettingsScene x, EditorBuildSettingsScene y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.enabled == y.enabled && x.path == y.path && x.guid.Equals(y.guid);
        }

        public int GetHashCode(EditorBuildSettingsScene obj)
        {
            unchecked
            {
                var hashCode = obj.enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.path != null ? obj.path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.guid.GetHashCode();
                return hashCode;
            }
        }
    }
}
