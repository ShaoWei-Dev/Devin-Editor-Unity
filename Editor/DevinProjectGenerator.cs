using System.IO;
using System.Linq;
using UnityEditor;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Minimal project generator. Unity automatically generates .csproj/.sln files
    /// when Devin is selected as the external script editor.
    /// </summary>
    internal static class DevinProjectGenerator
    {
        public static void SyncAll()
        {
            // Project file generation is handled by CodeEditor.SetExternalScriptEditor()
            // in DevinEditor.Initialize() and DevinEditor.SyncAll().
            // This method is kept for compatibility with the IExternalCodeEditor interface.
        }

        public static void SyncIfNeeded(string[] changedFiles)
        {
            if (changedFiles == null || changedFiles.Length == 0)
                return;

            // Only trigger a refresh if C# or assembly definition files changed.
            var relevantExtensions = new[] { ".cs", ".asmdef", ".asmref" };
            if (changedFiles.Any(path => relevantExtensions.Contains(Path.GetExtension(path).ToLowerInvariant())))
            {
                AssetDatabase.Refresh();
            }
        }
    }
}
