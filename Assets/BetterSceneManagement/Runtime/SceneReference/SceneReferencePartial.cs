#if UNITY_EDITOR
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.Runtime
{
    public partial class SceneReference : ISerializationCallbackReceiver
    {
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            EditorApplication.delayCall += DelayCall;
        }

        private void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;
            if (!Validate())
            {
                return;
            }

            var sceneAssetPath = AssetDatabase.GUIDToAssetPath(_guid);

            if (sceneAssetPath.IsNullOrEmpty())
            {
                return;
            }
            
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
            if (!_name.CompareOrdinal(sceneAsset.name) || !_fullPath.CompareOrdinal(sceneAssetPath))
            {
                return;
            }

            _name = sceneAsset.name;
            _fullPath = sceneAssetPath;
        }
    }
}
#endif