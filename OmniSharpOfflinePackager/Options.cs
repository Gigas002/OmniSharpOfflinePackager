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
        /// <para>For example: 1.21.2.</para>
        /// </summary>
        [Option('p', "package-version", Required = true, HelpText = "Package version to create.")]
        public string PackageVersion { get; set; }

        #endregion

        #region Optional

        /// <summary>
        /// Full path to directory, where ready packages will be located.
        /// <para>If not set – ready packages are located in cloned repo’s directory.</para>
        /// </summary>
        [Option('o', "output", Required = false, HelpText = "Directory for ready packages.")]
        public string OutputDirectoryPath { get; set; }

        #endregion
    }
}
