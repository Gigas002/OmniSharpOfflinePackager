namespace OmniSharpOfflinePackager.Enums
{
    /// <summary>
    /// Git commands.
    /// </summary>
    internal static class Git
    {
        /// <summary>
        /// Get string for git to clone OmniSharp repo.
        /// </summary>
        /// <param name="packageVersion">Package version to clone.</param>
        /// <param name="repoUri">Repo's uri (in case it changed over time).</param>
        /// <returns>String with arguments for Git process.</returns>
        internal static string GetCloneString(string packageVersion, string repoUri = "https://github.com/OmniSharp/omnisharp-vscode.git") =>
            $"clone --single-branch --branch v{packageVersion} {repoUri}";
    }
}
