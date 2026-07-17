using UnityEditor;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Backing preferences for the Devin Desktop IDE integration.
    /// </summary>
    internal static class DevinPreferences
    {
        private const string Prefix = "com.shaowei.devin.editor.";
        private const string DevinIdePathKey = Prefix + "DevinIdePath";
        private const string AutoGenerateRulesKey = Prefix + "AutoGenerateRules";
        private const string AutoGenerateContextKey = Prefix + "AutoGenerateContext";
        private const string IncludeExternalPackagesKey = Prefix + "IncludeExternalPackages";

        public static string DevinIdePath
        {
            get => EditorPrefs.GetString(DevinIdePathKey, "");
            set => EditorPrefs.SetString(DevinIdePathKey, value ?? "");
        }

        public static bool AutoGenerateRules
        {
            get => EditorPrefs.GetBool(AutoGenerateRulesKey, true);
            set => EditorPrefs.SetBool(AutoGenerateRulesKey, value);
        }

        public static bool AutoGenerateContext
        {
            get => EditorPrefs.GetBool(AutoGenerateContextKey, true);
            set => EditorPrefs.SetBool(AutoGenerateContextKey, value);
        }

        public static bool IncludeExternalPackages
        {
            get => EditorPrefs.GetBool(IncludeExternalPackagesKey, false);
            set => EditorPrefs.SetBool(IncludeExternalPackagesKey, value);
        }
    }
}
