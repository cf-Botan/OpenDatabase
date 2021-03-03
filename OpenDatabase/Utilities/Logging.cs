using BepInEx.Logging;
namespace OpenDatabase
{
    public static class Logger
    {
        private static ManualLogSource _logsource = BepInEx.Logging.Logger.CreateLogSource("OpenDatabase");
        public static void Log(object obj, LogLevel level = LogLevel.Info)
        {

            _logsource.Log(level, obj);
        }

        public static void LogError(object obj)
        {
            _logsource.LogError(obj);
        }
    }
}
