using System;
using System.Collections.Generic;

namespace Better.SceneManagement.Runtime
{
    public class SceneReferenceComparer : IEqualityComparer<SceneReference>
    {
        public static readonly SceneReferenceComparer Comparer = new();

        public bool Equals(SceneReference x, SceneReference y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            return x.Name == y.Name && x.FullPath == y.FullPath;
        }

        public int GetHashCode(SceneReference obj)
        {
            return HashCode.Combine(obj.Name, obj.FullPath);
        }
    }
}