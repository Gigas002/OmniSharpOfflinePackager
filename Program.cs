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
                throw new NotSupportedException(Strings.OsNotSupported);

            //Start the timer.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Start the work.
            await BuildPackage().ConfigureAwait(false);

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
        private static async ValueTask BuildPackage()
        {
            //Clone repo
            await GitProcess(Git.GetCloneString(PackageVersion)).ConfigureAwait(false);

            //Instal dependencies
            await NpmProcess(Npm.Install, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Comment string if packaging on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                await CommentThrowStatement(OmniSharp.OfflinePackagingTasksPath, OmniSharp.StringToComment).ConfigureAwait(false);

            //Compile package
            await NpmProcess(Npm.Compile, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Create package
            await NpmProcess(Npm.Gulp, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Move ready packages
            //Windows
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.GetWindowsPackageName(PackageVersion)),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.GetWindowsPackageName(PackageVersion)));

            //Linux
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.GetLinuxPackageName(PackageVersion)),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.GetLinuxPackageName(PackageVersion)));

            //OSX
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.GetMacOsPackageName(PackageVersion)),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.GetMacOsPackageName(PackageVersion)));
        }

        /// <summary>
        /// Starts Git process.
        /// </summary>
        /// <param name="args">Git process's args.</param>
        /// <returns></returns>
        private static async ValueTask GitProcess(string args) => await Task.Run(() =>
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo(nameof(Git))
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
        private static async ValueTask NpmProcess(string args, string workingDirectory) => await Task.Run(() =>
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo(nameof(Npm))
                {
                    Arguments = args, CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true,
                    UseShellExecute = false, WorkingDirectory = workingDirectory
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
        private static async ValueTask CommentThrowStatement(string offlinePackagingTasksPath, string lookForString) =>
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
            if (string.IsNullOrWhiteSpace(options.PackageVersion))
                throw new NullReferenceException(Strings.CommandLineParameterIsNullOrWhitespace);

            if (string.IsNullOrWhiteSpace(options.OutputDirectoryPath))
                throw new NullReferenceException(Strings.CommandLineParameterIsNullOrWhitespace);

            PackageVersion = options.PackageVersion;

            try
            {
                OutputDirectoryInfo = new DirectoryInfo(options.OutputDirectoryPath);
                OutputDirectoryInfo.Create();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                IsParsingErrors = true;
            }
        }
    }
}
