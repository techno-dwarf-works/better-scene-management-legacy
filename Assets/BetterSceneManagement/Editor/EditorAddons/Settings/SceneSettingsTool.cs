using Better.EditorTools.SettingsTools;
using Better.SceneManagement.Runtime;

namespace Better.SceneManagement.EditorAddons.Settings
{
    internal class SceneSettingsTool : ProjectSettingsTools<SceneLoaderSettings>
    {
        public const string SettingMenuItem = "Scene Management";
        public const string MenuItemPrefix = ProjectSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;
        
        public SceneSettingsTool() : base(nameof(SceneManagement), SettingMenuItem)
        {
        }
    }
}