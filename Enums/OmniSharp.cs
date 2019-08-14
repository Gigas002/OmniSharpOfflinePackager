namespace OmniSharpOfflinePackager.Enums
{
    internal static class OmniSharp
    {
        internal static readonly string OmniSharpDirectoryPath = "omnisharp-vscode";

        internal static string OfflinePackagingTasksPath
        {
            get => $"{OmniSharpDirectoryPath}/tasks/offlinePackagingTasks.ts";
        }

        internal static readonly string StringToComment = "        throw new Error('Do not build offline packages on windows. Runtime executables will not be marked executable in *nix packages.');";

        internal static readonly string MacOsPackageName = "csharp.1.21.0-darwin-x86_64.vsix";

        internal static readonly string WindowsPackageName = "csharp.1.21.0-win32-x86_64.vsix";

        internal static readonly string LinuxPackageName = "csharp.1.21.0-linux-x86_64.vsix";
    }
}
