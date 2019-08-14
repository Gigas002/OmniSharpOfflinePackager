using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq;
using CommandLine;
using OmniSharpOfflinePackager.Enums;

namespace OmniSharpOfflinePackager
{
    internal static class Program
    {
        #region Properties

        private static string PackageVersion { get; set; }

        private static bool IsParsingErrors { get; set; }

        private static DirectoryInfo OutputDirectoryInfo { get; set; }

        #endregion

        private static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(ParseConsoleOptions)
                  .WithNotParsed(error => IsParsingErrors = true);

            //Some additional checks
            if (IsParsingErrors) return;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                !RuntimeInformation.IsOSPlatform(OSPlatform.Linux) &&
                !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                throw new NotSupportedException(Strings.OsNotSupported);

            //Start the timer
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Start the work
            await BuildPackage();

            //Stop timer and print info
            stopwatch.Stop();
            Console.WriteLine(Strings.Done);
            Console.WriteLine(Strings.GetElapsedTimeString(stopwatch));
        }

        private static async ValueTask BuildPackage()
        {
            //Clone repo
            await GitProcess(Git.GetCloneString(PackageVersion));

            //Instal dependencies
            await NpmProcess(Npm.Install, OmniSharp.OmniSharpDirectoryPath);

            //Comment string if packaging on windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                await CommentThrowStatement(OmniSharp.OfflinePackagingTasksPath, OmniSharp.StringToComment);

            //Compile package
            await NpmProcess(Npm.Compile, OmniSharp.OmniSharpDirectoryPath);

            //Create package
            await NpmProcess(Npm.Gulp, OmniSharp.OmniSharpDirectoryPath);

            //Move ready packages
            //Windows
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.WindowsPackageName),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.WindowsPackageName));

            //Linux
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.LinuxPackageName),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.LinuxPackageName));

            //OSX
            File.Move(Path.Combine(OmniSharp.OmniSharpDirectoryPath, OmniSharp.MacOsPackageName),
                      Path.Combine(OutputDirectoryInfo.FullName, OmniSharp.MacOsPackageName));
        }

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
        });

        private static async ValueTask NpmProcess(string args, string workingDirectory) => await Task.Run(() =>
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo(nameof(Npm))
                {
                    Arguments = args, CreateNoWindow = true, RedirectStandardInput = false, RedirectStandardOutput = false,
                    UseShellExecute = true, WorkingDirectory = workingDirectory
                }
            };

            process.Start();
            process.WaitForExit();
        });

        private static async ValueTask CommentThrowStatement(string offlinePackagingTasksPath, string lookForString) =>
            await File.WriteAllLinesAsync(offlinePackagingTasksPath,
                                          (await File.ReadAllLinesAsync(offlinePackagingTasksPath)).Select(readString =>
                                                                                                               readString ==
                                                                                                               lookForString
                                                                                                                   ? $"//{readString}"
                                                                                                                   : readString));

        private static void ParseConsoleOptions(Options options)
        {
            if (string.IsNullOrWhiteSpace(options.PackageVersion))
                throw new NullReferenceException(Strings.ParameterIsNullOrWhitespace());

            if (string.IsNullOrWhiteSpace(options.OutputDirectoryPath))
                throw new NullReferenceException(Strings.ParameterIsNullOrWhitespace());

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
