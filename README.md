# Code Editor Package for Devin IDE

Unity Editor integration for the Devin Desktop IDE. This package registers Devin as an external script editor, generates `.csproj` and `.sln` files for IntelliSense, auto-discovers installations, and copies Unity code style rules to `.devin/rules/`.

## Using the Devin Editor package

To use the package, go to **Edit** > **Preferences** > **External Tools** > **External Script Editor** and select **Devin**. When you select this option, the window reloads and displays Devin-specific settings.

## Generate project files

Unity automatically generates `.csproj` and `.sln` files when Devin is set as the external script editor. Click **Regenerate project files** in the External Tools preferences to refresh them manually.

## Installation

### From Git URL (recommended)

In Unity, open **Window** > **Package Manager**, click **+** > **Add package from git URL...**, and enter:

```text
https://github.com/ShaoWei-Dev/Devin-Editor-Unity.git
```

### Local/embedded

1. Clone or copy this repository into your project's `Packages/` folder as `Packages/com.shaowei.devin.editor`.
2. Unity will automatically pick it up.

## Requirements

- Unity 2021.3 or newer.

## Features

- **External editor integration**: Registers Devin as an external script editor in Unity via `IExternalCodeEditor`.
- **Auto-discovery**: Automatically finds Devin Desktop IDE installations on macOS, Windows, and Linux.
- **Environment variable override**: Set `DEVIN_PATH` or `DEVIN_EXE` to specify the installation path.
- **Line/column support**: Opens scripts in Devin at the requested line and column.
- **Project file generation**: Generates `.csproj` (SDK-style) and `.sln` files for IntelliSense, debugging, and analyzers.
- **Auto-copy project rules**: Copies `UnityCodeStyleInstructions.md` and `UnityPerformanceOptimizationInstructions.md` to `.devin/rules/` when opening a project.
- **External package support**: Generates `.devin.code-workspace` files for editing external Unity packages.
- **VS Code settings**: Creates `.vscode/settings.json`, `.vscode/launch.json`, and `.vscode/extensions.json` with Unity-appropriate defaults.
- **Preferences**: Configurable options for auto-copying rules, including external packages, and showing the Library folder.
