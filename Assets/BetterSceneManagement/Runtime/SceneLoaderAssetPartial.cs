#if UNITY_EDITOR
using Better.Extensions.Runtime;
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
            if (!this.Validate()) return;
            var sceneAssetPath = AssetDatabase.GUIDToAssetPath(guid);
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
            if (!name.FastEquals(sceneAsset.name) || !fullPath.FastEquals(sceneAssetPath)) return;
            name = sceneAsset.name;
            fullPath = sceneAssetPath;
        }
    }
}
#endif