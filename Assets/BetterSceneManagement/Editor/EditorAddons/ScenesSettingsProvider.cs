using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
{
    internal class ScenesSettingsProvider : DefaultProjectSettingsProvider<SceneSystemSettings>
    {
        public ScenesSettingsProvider() : base(SceneSystemSettings.Path)
        {
        }

        [MenuItem(SceneSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + SceneSystemSettings.Path);
        }
    }
}