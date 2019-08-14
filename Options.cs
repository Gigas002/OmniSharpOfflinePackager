using CommandLine;

// ReSharper disable All

namespace OmniSharpOfflinePackager
{
    public class Options
    {
        #region Required

        [Option("package-version", Required = true, HelpText = "Package version to create.")]
        public string PackageVersion { get; set; }

        [Option('o', "output", Required = true, HelpText = "Directory for ready packages.")]
        public string OutputDirectoryPath { get; set; }

        #endregion
    }
}
