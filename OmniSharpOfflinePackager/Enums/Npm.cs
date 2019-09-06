namespace OmniSharpOfflinePackager.Enums
{
    /// <summary>
    /// Npm commands.
    /// </summary>
    internal static class Npm
    {
        /// <summary>
        /// Install command.
        /// </summary>
        internal const string Install = "i";

        /// <summary>
        /// Compile command.
        /// </summary>
        internal const string Compile = "run compile";

        /// <summary>
        /// Gulp command.
        /// </summary>
        internal const string Gulp = "run gulp vsix:offline:package";
    }
}
