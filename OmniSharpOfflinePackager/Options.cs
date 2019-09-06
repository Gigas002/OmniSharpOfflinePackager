using CommandLine;

// ReSharper disable All

namespace OmniSharpOfflinePackager
{
    /// <summary>
    /// Options from command line.
    /// </summary>
    public class Options
    {
        #region Required

        /// <summary>
        /// Package version to create. See tags in omnisharp-vscode repository and enter number without "v".
        /// For example: 1.21.2
        /// </summary>
        [Option("package-version", Required = true, HelpText = "Package version to create.")]
        public string PackageVersion { get; set; }

        /// <summary>
        /// Full path to directory, where ready packages will be located.
        /// </summary>
        [Option('o', "output", Required = true, HelpText = "Directory for ready packages.")]
        public string OutputDirectoryPath { get; set; }

        #endregion
    }
}
