using System.Reflection;
using Better.EditorTools.EditorAddons.Attributes;
using Better.EditorTools.EditorAddons.Drawers.Base;
using Better.EditorTools.Runtime.Attributes;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons.Drawers
{
    [MultiCustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : MultiFieldDrawer<SceneReferenceUtilityWrapper>
    {
        public SceneReferenceDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            _wrappers ??= GenerateCollection();
            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
        }

        protected override WrapperCollection<SceneReferenceUtilityWrapper> GenerateCollection()
        {
            return new();
        }

        protected override void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            wrapper.DrawField(position, property, label);
        }

        private SceneReferenceUtilityWrapper GetWrapper(SerializedProperty property)
        {
            var cache = ValidateCachedProperties(property, SceneReferenceUtility.Instance);
            return cache.Value.Wrapper;
        }
    }
}