using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
{
    internal class SceneSystemSettingsProvider : DefaultProjectSettingsProvider<SceneSystemSettings>
    {
        public SceneSystemSettingsProvider() : base(SceneSystemSettings.Path)
        {
        }

        [MenuItem(SceneSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + SceneSystemSettings.Path);
        }
    }
}