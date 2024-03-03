using Better.EditorTools.EditorAddons.Utilities;
using Better.SceneManagement.EditorAddons.Extensions;
using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons.Drawers
{
    public class SceneReferenceUtilityWrapper : UtilityWrapper
    {
        private const string NamePropertyPath = "name";
        private const string FullPathPropertyPath = "fullPath";
        private const string GUIDPropertyPath = "guid";

        public override void Deconstruct()
        {
        }

        public void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            if (!property.type.Equals(nameof(SceneReference)))
            {
                EditorGUILayout.LabelField($"{property.type} is not supported");
            }

            var fullPath = property.FindPropertyRelative(FullPathPropertyPath);
            if (fullPath.propertyType == SerializedPropertyType.String)
            {
                SceneAsset currentScene;
                var guidPathProperty = property.FindPropertyRelative(GUIDPropertyPath);
                if (ValidateSceneGUID(guidPathProperty.stringValue, fullPath.stringValue, out var resolvedScene))
                {
                    currentScene = resolvedScene;
                    UpdateProperty(property, currentScene.ToSceneReference());
                }
                else
                {
                    currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(fullPath.stringValue);
                }

                var newScene =
                    EditorGUI.ObjectField(position, label, currentScene, typeof(SceneAsset), false) as SceneAsset;

                if (currentScene == newScene) return;
                var loaderAsset = newScene.ToSceneReference();

                UpdateProperty(property, loaderAsset);

                property.serializedObject.ApplyModifiedProperties();
                ScenesValidator.ValidateSceneInBuildSettings(loaderAsset);
            }
            else
            {
                EditorGUILayout.LabelField($"{property.type} is not supported");
            }
        }

        private static bool ValidateSceneGUID(string guid, string fullPath, out SceneAsset sceneLoaderAsset)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!fullPath.Equals(path))
                {
                    sceneLoaderAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    if (sceneLoaderAsset != null)
                    {
                        return true;
                    }
                }
            }

            sceneLoaderAsset = null;
            return false;
        }

        private static void UpdateProperty(SerializedProperty property, SceneReference reference)
        {
            var guidPathProperty = property.FindPropertyRelative(GUIDPropertyPath);
            guidPathProperty.stringValue = reference.Guid;

            var fullPathProperty = property.FindPropertyRelative(FullPathPropertyPath);
            fullPathProperty.stringValue = reference.FullPath;

            var nameProperty = property.FindPropertyRelative(NamePropertyPath);
            nameProperty.stringValue = reference.Name;
        }
    }
}