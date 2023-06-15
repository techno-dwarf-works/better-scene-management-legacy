using Better.EditorTools.SettingsTools;
using Better.SceneManagement.Runtime;

namespace Better.SceneManagement.EditorAddons.Settings
{
    internal class SceneSettingsTool : BetterSettingsTools<SceneLoaderSettings>
    {
        public const string SettingMenuItem = "Scene Management";
        public const string MenuItemPrefix = BetterSettingsRegisterer.BetterPrefix + "/" + SettingMenuItem;
        
        public SceneSettingsTool() : base(nameof(SceneManagement), SettingMenuItem)
        {
        }
    }
}