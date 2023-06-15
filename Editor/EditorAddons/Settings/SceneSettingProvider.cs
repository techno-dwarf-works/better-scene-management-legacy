using System.Collections.Generic;
using Better.EditorTools.SettingsTools;
using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons.Settings
{
    internal class SceneSettingsTool : BetterSettingsTools<SceneLoaderSettings>
    {
        public const string SettingMenuItem = "Scene Management";
        
        public SceneSettingsTool() : base(nameof(SceneManagement), SettingMenuItem)
        {
        }
    }
    
    internal class SceneSettingProvider : BetterSettingsProvider<SceneLoaderSettings>
    {
        public SceneSettingProvider() : base(BetterSettingsToolsContainer<SceneSettingsTool>.Instance, SettingsScope.Project)
        {
            keywords = new HashSet<string>(new[] { "Better", "Scene", "Management" });
        }

        [MenuItem(BetterSettingsRegisterer.BetterPrefix + "/" + SceneSettingsTool.SettingMenuItem + "/" + BetterSettingsRegisterer.HighlightPrefix, false)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(BetterSettingsToolsContainer<SceneSettingsTool>.Instance.ProjectSettingKey);
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.PropertyField(_settingsObject.FindProperty("intermediateScene"));
            EditorGUILayout.PropertyField(_settingsObject.FindProperty("timeInIntermediateScene"));

            SceneSettingsValidator.ValidateScenesSettings(_settings);
        }
    }
}