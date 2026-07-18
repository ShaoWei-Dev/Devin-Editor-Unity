# Code Editor Package for Devin IDE

## [1.0.0] - 2026-07-17

### Added

- Initial release of Devin Editor for Unity.
- Registers Devin Desktop IDE as an external script editor via `IExternalCodeEditor`.
- Auto-discovery of Devin IDE installations on macOS, Windows, and Linux.
- `DEVIN_PATH`/`DEVIN_EXE` environment variable override for installation discovery.
- Opens scripts with line and column support.
- SDK-style `.csproj` and `.sln` generation for IntelliSense, debugging, and analyzers.
- Auto-copy of `UnityCodeStyleInstructions.md` and `UnityPerformanceOptimizationInstructions.md` to `.devin/rules/`.
- `.devin.code-workspace` generation for editing external Unity packages.
