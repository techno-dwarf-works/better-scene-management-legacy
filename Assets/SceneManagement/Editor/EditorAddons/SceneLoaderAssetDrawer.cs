using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SceneManagement.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SceneManagement.EditorAddons
{
    [CustomPropertyDrawer(typeof(SceneLoaderAsset))]
    public class SceneLoaderAssetDrawer : PropertyDrawer
    {
        private bool errorThrown;
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
                var currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(fullPath.stringValue);
                var newScene =
                    EditorGUI.ObjectField(position, label, currentScene, typeof(SceneAsset), false) as SceneAsset;

                if (currentScene != newScene)
                {
                    errorThrown = false;
                    var loaderAsset = newScene.ToSceneLoaderAsset();
                    
                    UpdateProperty(property, loaderAsset);

                    property.serializedObject.ApplyModifiedProperties();
                    CheckBuildScene(loaderAsset.FullPath, newScene);
                }
            }
            else
            {
                EditorGUILayout.LabelField($"{property.type} is not supported");
            }
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

        private void CheckBuildScene(string path, Object sceneToCheck)
        {
            if (sceneToCheck == null) return;
            var buildScene = EditorBuildSettings.scenes.FirstOrDefault(x => x.path == path);
            if (errorThrown) return;

            if (buildScene == null)
                Debug.LogError($"Scene <b>{sceneToCheck.name}</b> not in build. Add scene to SceneLoaderSettings.",
                    sceneToCheck);
            else if (!buildScene.enabled)
                Debug.LogError($"Scene <b>{sceneToCheck.name}</b> not enabled in build settings", sceneToCheck);
            errorThrown = true;
        }
    }
}