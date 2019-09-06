namespace OmniSharpOfflinePackager.Enums
{
    /// <summary>
    /// Mostly some paths.
    /// </summary>
    internal static class OmniSharp
    {
        /// <summary>
        /// Path to cloned omnisharp-vscode repo.
        /// </summary>
        internal const string OmniSharpDirectoryPath = "omnisharp-vscode";

        /// <summary>
        /// Path to "offlinePackagingTasks.ts" file.
        /// </summary>
        internal static string OfflinePackagingTasksPath => $"{OmniSharpDirectoryPath}/tasks/offlinePackagingTasks.ts";

        /// <summary>
        /// String to comment in "offlinePackagingTasks.ts" file.
        /// </summary>
        internal const string StringToComment = "        throw new Error('Do not build offline packages on windows. Runtime executables will not be marked executable in *nix packages.');";

        /// <summary>
        /// Gets MacOs package name.
        /// </summary>
        /// <param name="packageVersion">Created package version.</param>
        /// <returns>e.g.: csharp.1.21.2-darwin-x86_64.vsix</returns>
        internal static string GetMacOsPackageName(string packageVersion) => $"csharp.{packageVersion}-darwin-x86_64.vsix";

        /// <summary>
        /// Gets Windows package name.
        /// </summary>
        /// <param name="packageVersion">Created package version.</param>
        /// <returns>e.g.: csharp.1.21.2-win32-x86_64.vsix</returns>
        internal static string GetWindowsPackageName(string packageVersion) => $"csharp.{packageVersion}-win32-x86_64.vsix";

        /// <summary>
        /// Gets Linux package name.
        /// </summary>
        /// <param name="packageVersion">Created package version.</param>
        /// <returns>e.g.: csharp.1.21.2-linux-x86_64.vsix</returns>
        internal static string GetLinuxPackageName(string packageVersion) => $"csharp.{packageVersion}-linux-x86_64.vsix";
    }
}
