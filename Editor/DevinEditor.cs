using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Registers Devin as an external code editor in Unity, mirroring the TRAE-Editor-Unity pattern.
    /// </summary>
    [InitializeOnLoad]
    public class DevinEditor : IExternalCodeEditor
    {
        private static readonly List<DevinInstallation> _installations = new List<DevinInstallation>();

        CodeEditor.Installation[] IExternalCodeEditor.Installations => _installations
            .Select(i => new CodeEditor.Installation { Name = i.Name, Path = i.Path })
            .ToArray();

        static DevinEditor()
        {
            if (!UnityInstallation.IsMainUnityEditorProcess)
                return;

            RefreshInstallations();
            CodeEditor.Register(new DevinEditor());
        }

        private static void RefreshInstallations()
        {
            _installations.Clear();
            _installations.AddRange(DevinInstallation.GetInstallations());
        }

        public void CreateIfDoesntExist()
        {
            SyncAll();
        }

        public void Initialize(string editorInstallationPath)
        {
            RefreshInstallations();
        }

        public bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
        {
            installation = default;
            if (string.IsNullOrEmpty(editorPath))
                return false;

            var normalized = DevinInstallation.NormalizePath(editorPath);
            if (DevinInstallation.IsValidInstallation(normalized))
            {
                installation = new CodeEditor.Installation { Name = "Devin", Path = normalized };
                return true;
            }

            return false;
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            var package = UnityEditor.PackageManager.PackageInfo.FindForAssembly(GetType().Assembly);
            if (package != null)
            {
                var style = new GUIStyle
                {
                    richText = true,
                    margin = new RectOffset(0, 4, 0, 0)
                };
                GUILayout.Label($"<size=10><color=grey>{package.displayName} v{package.version} enabled</color></size>", style);
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Devin IDE", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            var idePath = EditorGUILayout.TextField("Devin IDE Path", DevinPreferences.DevinIdePath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                var selected = EditorUtility.OpenFilePanel("Select Devin IDE", DevinPreferences.DevinIdePath, "");
                if (!string.IsNullOrEmpty(selected))
                    idePath = selected;
            }
            if (GUILayout.Button("Auto Detect", GUILayout.Width(80)))
            {
                RefreshInstallations();
                var detected = _installations.FirstOrDefault();
                if (detected != null)
                    idePath = detected.Path;
            }
            EditorGUILayout.EndHorizontal();

            if (idePath != DevinPreferences.DevinIdePath)
                DevinPreferences.DevinIdePath = idePath;

            if (!DevinInstallation.IsAvailable)
            {
                EditorGUILayout.HelpBox(
                    "Devin Desktop IDE not detected. Install Devin or set the path above. " +
                    "Until then, Unity will not be able to open scripts in Devin.",
                    MessageType.Warning);
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Project Files", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());
            rect.width = 252;
            if (GUI.Button(rect, "Regenerate project files"))
                SyncAll();

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Devin Context", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            var autoRules = EditorGUILayout.Toggle(
                new GUIContent("Auto-generate .devin/rules.md", "Write Unity-specific guidance to .devin/rules.md on editor load."),
                DevinPreferences.AutoGenerateRules);
            if (autoRules != DevinPreferences.AutoGenerateRules)
                DevinPreferences.AutoGenerateRules = autoRules;

            var autoContext = EditorGUILayout.Toggle(
                new GUIContent("Auto-generate .devin/context.md", "Write project summary to .devin/context.md on editor load."),
                DevinPreferences.AutoGenerateContext);
            if (autoContext != DevinPreferences.AutoGenerateContext)
                DevinPreferences.AutoGenerateContext = autoContext;

            EditorGUI.indentLevel--;
        }

        public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles, string[] movedFromFiles, string[] importedFiles)
        {
            var all = addedFiles
                .Union(deletedFiles)
                .Union(movedFiles)
                .Union(movedFromFiles)
                .Union(importedFiles)
                .ToArray();

            DevinProjectGenerator.SyncIfNeeded(all);
        }

        public void SyncAll()
        {
            // When Devin is the active external editor, re-setting it triggers Unity to regenerate .csproj/.sln files.
            var currentPath = CodeEditor.CurrentEditorInstallation;
            if (!string.IsNullOrEmpty(currentPath))
                CodeEditor.SetExternalScriptEditor(currentPath);
        }

        public bool OpenProject(string path, int line, int column)
        {
            var editorPath = CodeEditor.CurrentEditorInstallation;
            if (!DevinInstallation.TryGetInstallation(editorPath, out var installation))
            {
                UnityEngine.Debug.LogWarning($"[Devin] Devin Desktop IDE not found at {editorPath}. Please set it in Edit > Preferences > External Tools.");
                return false;
            }

            if (!IsSupportedPath(path))
                return false;

            SyncAll();
            return installation.Open(path, line, column);
        }

        private static bool IsSupportedPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return true;

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext == ".cs"
                || ext == ".uxml"
                || ext == ".uss"
                || ext == ".shader"
                || ext == ".compute"
                || ext == ".cginc"
                || ext == ".hlsl"
                || ext == ".glslinc"
                || ext == ".asmdef"
                || ext == ".txt"
                || ext == ".xml"
                || ext == ".json"
                || ext == ".md";
        }
    }
}
