namespace OmniSharpOfflinePackager.Enums
{
    internal static class Git
    {
        internal static string GetCloneString(string packageVersion) =>
            $"clone --single-branch --branch v{packageVersion} https://github.com/OmniSharp/omnisharp-vscode.git";
    }
}
