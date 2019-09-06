﻿using System;
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
            await GitProcessAsync(Git.GetCloneString(PackageVersion)).ConfigureAwait(false);

            //Instal dependencies
            await NpmProcessAsync(Npm.Install, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Comment string if packaging on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                await CommentThrowStatementAsync(OmniSharp.OfflinePackagingTasksPath, OmniSharp.StringToComment).ConfigureAwait(false);

            //Compile package
            await NpmProcessAsync(Npm.Compile, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

            //Create package
            await NpmProcessAsync(Npm.Gulp, OmniSharp.OmniSharpDirectoryPath).ConfigureAwait(false);

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
        private static async ValueTask GitProcessAsync(string args) => await Task.Run(() =>
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
        private static async ValueTask NpmProcessAsync(string args, string workingDirectory) => await Task.Run(() =>
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
