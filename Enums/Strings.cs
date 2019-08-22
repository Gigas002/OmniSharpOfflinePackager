using System.Diagnostics;

namespace OmniSharpOfflinePackager.Enums
{
    //TODO: Move to Localization
    internal static class Strings
    {
        internal const string OsNotSupported = "Current OS is not supported";

        internal static string ParameterIsNullOrWhitespace() => "Command line parameter is null or white space";

        internal const string Done = "Done!";

        internal static string GetElapsedTimeString(Stopwatch stopwatch) =>
            $"Elapsed minutes:{stopwatch.Elapsed.Minutes}, elapsed seconds:" +
            $"{stopwatch.Elapsed.Seconds}, elapsed milliseconds:{stopwatch.Elapsed.Milliseconds}";
    }
}
