namespace OmniSharpOfflinePackager.Enums
{
    internal static class OmniSharp
    {
        internal const string OmniSharpDirectoryPath = "omnisharp-vscode";

        internal static string OfflinePackagingTasksPath => $"{OmniSharpDirectoryPath}/tasks/offlinePackagingTasks.ts";

        internal const string StringToComment = "        throw new Error('Do not build offline packages on windows. Runtime executables will not be marked executable in *nix packages.');";

        internal const string MacOsPackageName = "csharp.1.21.0-darwin-x86_64.vsix";

        internal const string WindowsPackageName = "csharp.1.21.0-win32-x86_64.vsix";

        internal const string LinuxPackageName = "csharp.1.21.0-linux-x86_64.vsix";
    }
}
