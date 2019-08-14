using System.Diagnostics;

namespace OmniSharpOfflinePackager.Enums
{
    internal static class Strings
    {
        internal static readonly string OsNotSupported = "Current OS is not supported";

        internal static string ParameterIsNullOrWhitespace(dynamic parameter) =>
            $"{nameof(parameter)} is null or white space";

        internal static readonly string Done = "Done!";

        internal static string GetElapsedTimeString(Stopwatch stopwatch) =>
            $"Elapsed minutes:{stopwatch.Elapsed.Minutes}, elapsed seconds:" + 
            $"{stopwatch.Elapsed.Seconds}, elapsed milliseconds:{stopwatch.Elapsed.Milliseconds}";
    }
}
