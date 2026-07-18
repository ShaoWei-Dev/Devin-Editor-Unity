/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Unity Technologies.
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using UnityEditor;

namespace Microsoft.Unity.VisualStudio.Editor
{
	internal static class DevinPreferences
	{
		private const string Prefix = "com.shaowei.devin.";
		private const string AutoCopyProjectRulesKey = Prefix + "AutoCopyProjectRules";
		private const string IncludeExternalPackagesKey = Prefix + "IncludeExternalPackages";
		private const string ShowLibraryKey = Prefix + "ShowLibrary";

		internal static bool AutoCopyProjectRules
		{
			get => EditorPrefs.GetBool(AutoCopyProjectRulesKey, true);
			set => EditorPrefs.SetBool(AutoCopyProjectRulesKey, value);
		}

		internal static bool IncludeExternalPackages
		{
			get => EditorPrefs.GetBool(IncludeExternalPackagesKey, true);
			set => EditorPrefs.SetBool(IncludeExternalPackagesKey, value);
		}

		internal static bool ShowLibrary
		{
			get => EditorPrefs.GetBool(ShowLibraryKey, true);
			set => EditorPrefs.SetBool(ShowLibraryKey, value);
		}
	}
}
