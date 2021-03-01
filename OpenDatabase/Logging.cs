using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDatabase
{
    public static class Logging
    {
        private static ManualLogSource _logsource = Logger.CreateLogSource("OpenDatabase");
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
