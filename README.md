# Code Editor Package for Devin IDE

Unity Editor integration for the [Devin](https://www.devin.ai/) Desktop IDE. This package registers Devin as an external script editor and generates project context files under `.devin/`.

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

- **External editor integration**: Registers Devin as an external script editor in Unity.
- **Auto-discovery**: Automatically finds Devin Desktop IDE installations on macOS, Windows, and Linux.
- **Manual path override**: Set the Devin executable path in External Tools preferences.
- **Line/column support**: Opens scripts in Devin at the requested line and column.
- **Project context generation**: Writes `.devin/rules.md` and `.devin/context.md` with Unity-specific guidance and project metadata.
- **Project file regeneration**: Refreshes `.csproj` and `.sln` files via Unity's built-in code editor pipeline.

## Generated context files

The package can generate two files under `.devin/`:

- `.devin/rules.md` — Unity coding conventions and project guidance for the Devin IDE.
- `.devin/context.md` — Project summary, package list, assembly definitions, scripts, and scenes.

These files are written automatically when the editor loads if enabled in the preferences. You can also create them manually from `Tools > Devin > Generate Project Context`.
