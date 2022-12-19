using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
{
    /// <summary>
    /// Editor for <seealso cref="SceneLoaderSettings"/> validation
    /// </summary>
    [CustomEditor(typeof(SceneLoaderSettings))]
    public class SceneLoaderSettingsEditor : Editor
    {
        private SceneLoaderSettings _sceneLoaderSettings;

        private void OnEnable()
        {
            _sceneLoaderSettings = (SceneLoaderSettings)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SceneSettingsValidator.ValidateScenesSettings(_sceneLoaderSettings);
        }
    }
}
