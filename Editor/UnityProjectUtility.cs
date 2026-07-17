using System.IO;
using UnityEditor;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Common utility methods for Unity project paths.
    /// </summary>
    internal static class UnityProjectUtility
    {
        public static string GetProjectRoot()
        {
            return Path.GetDirectoryName(Path.GetFullPath("ProjectSettings/ProjectSettings.asset"));
        }

        public static string GetAssetsPath()
        {
            return Path.GetFullPath("Assets");
        }

        public static string GetRelativePath(string absolutePath)
        {
            var root = GetProjectRoot();
            if (string.IsNullOrEmpty(root))
                return absolutePath;

            var rootUri = new System.Uri(root + Path.DirectorySeparatorChar);
            var pathUri = new System.Uri(absolutePath);
            var relativeUri = rootUri.MakeRelativeUri(pathUri);
            return System.Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
