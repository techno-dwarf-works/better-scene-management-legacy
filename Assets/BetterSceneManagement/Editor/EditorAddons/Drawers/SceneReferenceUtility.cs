using System;
using System.Collections.Generic;
using Better.EditorTools.EditorAddons.Utilities;
using Better.EditorTools.EditorAddons.WrappersTypeCollection;
using Better.SceneManagement.Runtime;

namespace Better.SceneManagement.EditorAddons.Drawers
{
    public class SceneReferenceUtility : BaseUtility<SceneReferenceUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            var collection = new WrappersFieldTypeCollection();
            collection.Add(typeof(SceneReference), typeof(SceneReferenceUtilityWrapper));
            
            return collection;
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            throw new NotImplementedException();
        }
    }
}