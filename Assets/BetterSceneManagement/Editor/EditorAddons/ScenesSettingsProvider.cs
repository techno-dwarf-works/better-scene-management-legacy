using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.SceneManagement.Runtime;
using UnityEditor;

namespace Better.SceneManagement.EditorAddons
{
    internal class ScenesSettingsProvider : DefaultProjectSettingsProvider<SceneSystemSettings>
    {
        public const string Path = PrefixConstants.BetterPrefix + "/" + nameof(SceneManagement);

        public ScenesSettingsProvider() : base(Path)
        {
        }

        [MenuItem(Path + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + Path);
        }
    }
}