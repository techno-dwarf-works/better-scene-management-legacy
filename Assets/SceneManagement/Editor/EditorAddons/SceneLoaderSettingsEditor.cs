using SceneManagement.Runtime;
using UnityEditor;

namespace SceneManagement.EditorAddons
{
    /// <summary>
    /// Editor for <seealso cref="SceneLoaderSettings"/> validation
    /// </summary>
    [CustomEditor(typeof(SceneLoaderSettings))]
    public class SceneLoaderSettingsEditor : UnityEditor.Editor
    {
        private SceneLoaderSettings _sceneLoaderSettings;

        private void OnEnable()
        {
            _sceneLoaderSettings = (SceneLoaderSettings)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SceneLoaderSettingsValidator.ValidateScenesLoaderSettings(_sceneLoaderSettings);
        }
    }
}
