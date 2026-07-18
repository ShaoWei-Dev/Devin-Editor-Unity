using System.Linq;
using Unity.CodeEditor;
using UnityEditor;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Unity Editor menu items for the Devin IDE integration.
    /// </summary>
    internal static class DevinMenuItems
    {
        [MenuItem("Tools/Devin/Regenerate Project Files", priority = 1)]
        private static void RegenerateProjectFiles()
        {
            CodeEditor.CurrentEditor?.SyncAll();
            UnityEngine.Debug.Log("[Devin] Regenerated project files.");
        }

        [MenuItem("Tools/Devin/Generate Project Context", priority = 2)]
        private static void GenerateProjectContext()
        {
            DevinProjectContext.GenerateAll();
        }

        [MenuItem("Tools/Devin/Refresh Devin IDE Detection", priority = 100)]
        private static void RefreshDevinDetection()
        {
            DevinInstallation.Refresh();
            var installation = DevinInstallation.GetInstallations().FirstOrDefault();
            var status = installation != null ? $"found at {installation.ExecutablePath}" : "not found";
            UnityEngine.Debug.Log($"[Devin] Devin IDE {status}");
        }
    }
}
