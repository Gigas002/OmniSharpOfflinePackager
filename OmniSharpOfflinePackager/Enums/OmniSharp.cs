namespace OmniSharpOfflinePackager.Enums
{
    internal static class OmniSharp
    {
        internal const string OmniSharpDirectoryPath = "omnisharp-vscode";

        internal static string OfflinePackagingTasksPath => $"{OmniSharpDirectoryPath}/tasks/offlinePackagingTasks.ts";

        internal const string StringToComment = "        throw new Error('Do not build offline packages on windows. Runtime executables will not be marked executable in *nix packages.');";

        internal static string GetMacOsPackageName(string packageVersion) => $"csharp.{packageVersion}-darwin-x86_64.vsix";

        internal static string GetWindowsPackageName(string packageVersion) => $"csharp.{packageVersion}-win32-x86_64.vsix";

        internal static string GetLinuxPackageName(string packageVersion) => $"csharp.{packageVersion}-linux-x86_64.vsix";
    }
}
