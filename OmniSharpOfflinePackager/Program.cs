using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq;
using CommandLine;
using OmniSharpOfflinePackager.Enums;
using OmniSharpOfflinePackager.Localization;

namespace OmniSharpOfflinePackager
{
    internal static class Program
    {
        #region Properties

        /// <summary>
        /// Package version to create from repo.
        /// </summary>
        private static string PackageVersion { get; set; }

        /// <summary>
        /// Show is were additional errors while parsing.
        /// </summary>
        private static bool IsParsingErrors { get; set; }

        /// <summary>
        /// Directory to ready packages.
        /// </summary>
        private static DirectoryInfo OutputDirectoryInfo { get; set; }

        #endregion

        private static async Task Main(string[] args)
        {
            //Try parse console args.
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(ParseConsoleOptions)
                  .WithNotParsed(error => IsParsingErrors = true);

            //Additional check.
            if (IsParsingErrors) return;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                !RuntimeInformation.IsOSPlatform(OSPlatform.Linux) &&
                !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine(Strings.OsNotSupported);

                return;
            }

            //Start the timer.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Start the work.
            try { await BuildPackageAsync().ConfigureAwait(false); }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

                #if DEBUG
                Console.WriteLine(exception.InnerException?.Message);
                #endif
            }

            //Stop timer and print info.
            stopwatch.Stop();
            Console.WriteLine(Strings.Done, Environment.NewLine, stopwatch.Elapsed.Days, stopwatch.Elapsed.Hours,
                              stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds,
                              stopwatch.Elapsed.Milliseconds);
        }

        /// <summary>
        /// Builds the package.
        /// </summary>
        /// <returns></returns>
        private static async ValueTask BuildPackageAsync()
        {
            //Clone repo
            Console.WriteLine(Strings.CloningTheRepo);
            await GitProcessAsync(await Git.GetCloneStringAsync(PackageVersion).ConfigureAwait(false)).ConfigureAwait(false);

            //Instal dependencies
            Console.WriteLine(Strings.InstallingNpmDependencies);
            await NpmProcessAsync(Npm.Install, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Comment string if packaging on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine(Strings.CommentingThrowStatement);
                await
                    CommentThrowStatementAsync(await OmniSharp.GetOfflinePackagingTasksPathAsync.ConfigureAwait(false),
                                               OmniSharp.StringToComment).ConfigureAwait(false);
            }

            //Compile package
            Console.WriteLine(Strings.CompilingThePackage);
            await NpmProcessAsync(Npm.Compile, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Create package
            Console.WriteLine(Strings.CreatingThePackage);
            await NpmProcessAsync(Npm.Gulp, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Move ready packages if necessary

            if (OutputDirectoryInfo == null) return;

            Console.WriteLine(Strings.MovingReadyPackages);

            //Windows
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath,
                                   await OmniSharp.GetWindowsPackageNameAsync(PackageVersion).ConfigureAwait(false)),
                      Path.Combine(OutputDirectoryInfo.FullName,
                                   await OmniSharp.GetWindowsPackageNameAsync(PackageVersion).ConfigureAwait(false)));

            //Linux
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath,
                                   await OmniSharp.GetLinuxPackageNameAsync(PackageVersion).ConfigureAwait(false)),
                      Path.Combine(OutputDirectoryInfo.FullName,
                                   await OmniSharp.GetLinuxPackageNameAsync(PackageVersion).ConfigureAwait(false)));

            //OSX
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, await OmniSharp.GetMacOsPackageNameAsync(PackageVersion).ConfigureAwait(false)),
                      Path.Combine(OutputDirectoryInfo.FullName, await OmniSharp.GetMacOsPackageNameAsync(PackageVersion).ConfigureAwait(false)));
        }

        /// <summary>
        /// Starts Git process.
        /// </summary>
        /// <param name="args">Git process's args.</param>
        /// <returns></returns>
        private static async ValueTask GitProcessAsync(string args) => await Task.Run(() =>
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo(nameof(Git).ToLowerInvariant())
                {
                    Arguments = args, CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();
        }).ConfigureAwait(false);

        /// <summary>
        /// Starts Npm process.
        /// </summary>
        /// <param name="args">Npm process's args.</param>
        /// <param name="workingDirectory">Cloned repo's path.</param>
        /// <returns></returns>
        private static async ValueTask NpmProcessAsync(string args, string workingDirectory) => await Task.Run(() =>
        {
            using Process process = new Process
            {
                //TODO: Fix UseShellExecute = false -> unable to run npm

                StartInfo = new ProcessStartInfo(nameof(Npm).ToLowerInvariant())
                {
                    Arguments = args, CreateNoWindow = true, RedirectStandardInput = false, RedirectStandardOutput = false,
                    UseShellExecute = true, WorkingDirectory = workingDirectory
                }
            };

            process.Start();
            process.WaitForExit();
        }).ConfigureAwait(false);

        /// <summary>
        /// Comment throw statement in package code (when building on Windows only).
        /// </summary>
        /// <param name="offlinePackagingTasksPath">Path to OfflinePackagingTasks file.</param>
        /// <param name="lookForString">String to find and comment.</param>
        /// <returns></returns>
        private static async ValueTask CommentThrowStatementAsync(string offlinePackagingTasksPath, string lookForString) =>
            await File.WriteAllLinesAsync(offlinePackagingTasksPath,
                                          (await File.ReadAllLinesAsync(offlinePackagingTasksPath)
                                                     .ConfigureAwait(false))
                                         .Select(readString =>
                                                     readString == lookForString ? $"//{readString}" : readString))
                      .ConfigureAwait(false);

        /// <summary>
        /// Parses console options and checks them on errors.
        /// </summary>
        /// <param name="options">Options from command line.</param>
        private static void ParseConsoleOptions(Options options)
        {
            //Package version
            if (string.IsNullOrWhiteSpace(options.PackageVersion))
            {
                Console.WriteLine(Strings.CommandLineParameterIsNullOrWhitespace);
                IsParsingErrors = true;
            }

            PackageVersion = options.PackageVersion;

            //Output directory
            if (string.IsNullOrWhiteSpace(options.OutputDirectoryPath)) return;

            try
            {
                OutputDirectoryInfo = new DirectoryInfo(options.OutputDirectoryPath);
                OutputDirectoryInfo.Create();
                OutputDirectoryInfo.Refresh();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

                #if DEBUG
                Console.WriteLine(exception.InnerException?.Message);
                #endif

                IsParsingErrors = true;
            }
        }
    }
}
