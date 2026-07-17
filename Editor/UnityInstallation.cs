using System;
using UnityEditor;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Helpers to detect the current Unity Editor process.
    /// </summary>
    internal static class UnityInstallation
    {
        public static bool IsMainUnityEditorProcess
        {
            get
            {
#if UNITY_2020_2_OR_NEWER
                if (AssetDatabase.IsAssetImportWorkerProcess())
                    return false;
#elif UNITY_2019_3_OR_NEWER
                if (UnityEditor.Experimental.AssetDatabaseExperimental.IsAssetImportWorkerProcess())
                    return false;
#endif

#if UNITY_2021_1_OR_NEWER
                if (UnityEditor.MPE.ProcessService.level == UnityEditor.MPE.ProcessLevel.Secondary)
                    return false;
#elif UNITY_2020_2_OR_NEWER
                if (UnityEditor.MPE.ProcessService.level == UnityEditor.MPE.ProcessLevel.Slave)
                    return false;
#endif

                return true;
            }
        }

        private static readonly Lazy<bool> _lazyIsInSafeMode = new Lazy<bool>(() =>
        {
            var ieu = typeof(EditorUtility);
            var pinfo = ieu.GetProperty("isInSafeMode", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (pinfo == null)
                return false;

            return Convert.ToBoolean(pinfo.GetValue(null));
        });

        public static bool IsInSafeMode => _lazyIsInSafeMode.Value;
    }
}
