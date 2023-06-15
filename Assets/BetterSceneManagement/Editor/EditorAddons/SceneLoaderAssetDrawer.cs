using Better.SceneManagement.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.SceneManagement.EditorAddons
{
    [CustomPropertyDrawer(typeof(SceneLoaderAsset))]
    public class SceneLoaderAssetDrawer : PropertyDrawer
    {
        private const string NamePropertyPath = "name";
        private const string FullPathPropertyPath = "fullPath";
        private const string GUIDPropertyPath = "guid";

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            if (!property.type.Equals(nameof(SceneLoaderAsset)))
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
                    UpdateProperty(property, currentScene.ToSceneLoaderAsset());
                }
                else
                {
                    currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(fullPath.stringValue);
                }

                var newScene =
                    EditorGUI.ObjectField(position, label, currentScene, typeof(SceneAsset), false) as SceneAsset;

                if (currentScene == newScene) return;
                var loaderAsset = newScene.ToSceneLoaderAsset();

                UpdateProperty(property, loaderAsset);

                property.serializedObject.ApplyModifiedProperties();
                SceneValidator.ValidateSceneInBuildSettings(loaderAsset);
            }
            else
            {
                EditorGUILayout.LabelField($"{property.type} is not supported");
            }
        }

        private static bool ValidateSceneGUID(string guid, string fullPath,
            out SceneAsset sceneLoaderAsset)
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

        private static void UpdateProperty(SerializedProperty property, SceneLoaderAsset loaderAsset)
        {
            var guidPathProperty = property.FindPropertyRelative(GUIDPropertyPath);
            guidPathProperty.stringValue = loaderAsset.Guid;

            var fullPathProperty = property.FindPropertyRelative(FullPathPropertyPath);
            fullPathProperty.stringValue = loaderAsset.FullPath;

            var nameProperty = property.FindPropertyRelative(NamePropertyPath);
            nameProperty.stringValue = loaderAsset.Name;
        }
    }
}