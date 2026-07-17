using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Devin.Editor.Unity
{
    /// <summary>
    /// Discovers and launches Devin Desktop IDE installations.
    /// </summary>
    internal class DevinInstallation
    {
        public string Name { get; set; } = "Devin";
        public string Path { get; set; }
        public Version Version { get; set; }

        private static readonly List<DevinInstallation> _installations = new List<DevinInstallation>();

        public static IEnumerable<DevinInstallation> GetInstallations()
        {
            Refresh();
            return _installations;
        }

        public static bool IsAvailable
        {
            get
            {
                Refresh();
                return _installations.Any();
            }
        }

        public static void Refresh()
        {
            _installations.Clear();

            foreach (var candidate in GetInstallationCandidates().Distinct())
            {
                if (TryDiscoverInstallation(candidate, out var installation))
                    _installations.Add(installation);
            }

            // Also respect a manually configured path that may not be cached yet.
            var preferencePath = NormalizePath(DevinPreferences.DevinIdePath);
            if (IsValidInstallation(preferencePath) && !_installations.Any(i => NormalizePath(i.Path) == preferencePath))
            {
                TryDiscoverInstallation(preferencePath, out var manualInstallation);
                if (manualInstallation != null)
                    _installations.Add(manualInstallation);
            }
        }

        private static IEnumerable<string> GetInstallationCandidates()
        {
            // 1. Respect user override from preferences.
            var overridePath = DevinPreferences.DevinIdePath;
            if (!string.IsNullOrEmpty(overridePath))
                yield return overridePath;

            // 2. Look for the Devin executable on PATH.
            var devinName = Application.platform == RuntimePlatform.WindowsEditor ? "Devin.exe" : "devin";
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(pathEnv))
            {
                foreach (var dir in pathEnv.Split(Path.PathSeparator))
                {
                    if (string.IsNullOrWhiteSpace(dir))
                        continue;
                    yield return Path.Combine(dir, devinName);
                }
            }

            // 3. Check well-known installation locations.
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // macOS application bundle.
            yield return "/Applications/Devin.app/Contents/MacOS/Devin";
            yield return "/Applications/Devin.app/Contents/MacOS/devin";

            // npm-style / local installs.
            yield return Path.Combine(home, ".local", "bin", devinName);
            yield return Path.Combine(home, ".devin", "bin", devinName);
            yield return Path.Combine(home, ".bun", "bin", devinName);
            yield return Path.Combine(home, ".nvm", "versions", "node", "default", "bin", devinName);

            // Windows common locations.
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            yield return Path.Combine(localAppData, "Programs", "devin", "Devin.exe");
            yield return Path.Combine(localAppData, "Programs", "Devin", "Devin.exe");
            yield return Path.Combine(localAppData, "devin", "Devin.exe");
            yield return Path.Combine(localAppData, "devin", "devin.exe");

            // Linux common locations.
            yield return "/usr/bin/devin";
            yield return "/bin/devin";
            yield return "/usr/local/bin/devin";
            yield return "/opt/devin/devin";
        }

        public static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;
            try
            {
                return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            catch
            {
                return path.Trim();
            }
        }

        public static bool IsValidInstallation(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

#if UNITY_EDITOR_OSX
            // On macOS Devin.app may be passed in; validate directory or executable.
            if (Directory.Exists(path) && path.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
                return File.Exists(Path.Combine(path, "Contents/MacOS/Devin")) || File.Exists(Path.Combine(path, "Contents/MacOS/devin"));
#endif
            return File.Exists(path);
        }

        public static bool TryGetInstallation(string editorPath, out DevinInstallation installation)
        {
            installation = null;
            if (string.IsNullOrEmpty(editorPath))
                return false;

            var normalized = NormalizePath(editorPath);
            if (!IsValidInstallation(normalized))
                return false;

            installation = new DevinInstallation { Name = "Devin", Path = normalized };
            return true;
        }

        public static bool TryDiscoverInstallation(string editorPath, out DevinInstallation installation)
        {
            installation = null;
            if (string.IsNullOrEmpty(editorPath))
                return false;

            var normalized = NormalizePath(editorPath);
            if (!IsValidInstallation(normalized))
                return false;

            installation = new DevinInstallation
            {
                Name = "Devin",
                Path = normalized,
                Version = new Version()
            };

            return true;
        }

        public bool Open(string path, int line, int column)
        {
            try
            {
                var arguments = BuildOpenArguments(path, line, column);
                var startInfo = new ProcessStartInfo(Path, arguments)
                {
                    WorkingDirectory = UnityProjectUtility.GetProjectRoot(),
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);
                UnityEngine.Debug.Log($"[Devin] Opened {path ?? "project"} in Devin.");
                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[Devin] Failed to open {path} in Devin: {ex.Message}");
                return false;
            }
        }

        private static string BuildOpenArguments(string path, int line, int column)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            // Default open convention used by VS Code / TRAE / many editors: "path:line:column"
            var escaped = path.Replace("\\", "\\\\").Replace("\"", "\\\"");
            var result = $"\"{escaped}\"";

            if (line >= 0)
            {
                result += $":{line}";
                if (column >= 0)
                    result += $":{column}";
            }

            return result;
        }
    }
}
