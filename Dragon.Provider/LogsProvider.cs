using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace Dragon.Provider
{
    public static class LogsProvider
    {
        public static void WriteErrorLog(object exception, object data)
        {
            MethodBase methodBase = new StackFrame(1).GetMethod();
            WriteLog(new Logs()
            {
                Message = exception.ToJson(),
                Type = Convert.ToString(LogTypes.Error),
                Data = data != null ? data.ToJson() : string.Empty,
                Method = string.Format("{0}.{1}", methodBase.DeclaringType.FullName, methodBase.Name)
            });
        }
        private static void WriteLog(Logs logs)
        {
            string logsDBPath = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName, $"{DateTime.Today:ddMMyyyy}{ConfigProvider.Settings.LightDataExtension}");
            logs.ToJsonFile(logsDBPath);
        }
        private enum LogTypes : byte { Debug, Error, Info, Warning }
        private class Logs
        {
            [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
            [Required] public DateTime Time { get; set; } = DateTime.Now.ToUniversalTime();
            [Required] public string Type { get; set; }
            public string Method { get; set; }
            public string Message { get; set; }
            public string Data { get; set; }
        }
    }
}
//public void WriteEndLog()
//{
//    MethodBase methodBase = new StackFrame(1).GetMethod();
//    WriteLog(new Logs()
//    {
//        Method = string.Format("{0}.{1}", methodBase.DeclaringType.FullName, methodBase.Name),
//        Type = Convert.ToString(LogTypes.Info),
//        Message = "End Execution",
//    });
//}
//public void WriteStartLog()
//{
//    MethodBase methodBase = new StackFrame(1).GetMethod();
//    WriteLog(new Logs()
//    {
//        Method = string.Format("{0}.{1}", methodBase.DeclaringType.FullName, methodBase.Name),
//        Type = Convert.ToString(LogTypes.Info),
//        Message = "Start Execution",
//    });
//}
//public void WriteWarningLog(string warning)
//{
//    MethodBase methodBase = new StackFrame(1).GetMethod();
//    WriteLog(new Logs()
//    {
//        Method = string.Format("{0}.{1}", methodBase.DeclaringType.FullName, methodBase.Name),
//        Type = Convert.ToString(LogTypes.Warning),
//        Message = warning,
//    });
//}
//public void WriteTrace(Logs logs, string filename)
//{
//    string logTracePath = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName, $"{filename}.txt");
//    lock (_fileLock)
//    {
//        using StreamWriter file = new(logTracePath, append: true);
//        file.WriteLine($"{logs.Type}{logs.Time,25}{null,5}{logs.Message}{null,5}{(logs.Data != null ? logs.Data.ToJson().Replace("\\", "") : "")}");
//        file.Dispose();
//    }
//}
//public void WriteTrace(object data, string filename)
//{
//    string logTracePath = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName, $"{filename}.txt");
//    lock (_fileLock)
//    {
//        using StreamWriter file = new(logTracePath, append: true);
//        file.Write(data.ToFormattedJson());
//        file.Dispose();
//    }
//}
//public static async Task<List<Logs>> ReadLogs()
//{
//    List<Logs> logs = null;
//    if (Directory.Exists(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName)))
//    {
//        foreach (string logsDBPath in Directory.GetFiles(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName)))
//        {
//            using LogsContext logsContext = new(new Connection { Database = logsDBPath, DatabaseType = (byte)DatabaseType.Sqlite });
//            if (await logsContext.Logs.AsNoTracking().AnyAsync())
//            {
//                logs ??= new List<Logs>();
//                logs.AddRange(logsContext.Logs);
//            }
//        }
//    }
//    return logs;
//}
//public static async Task<List<Logs>> ReadTodayLogs()
//{
//    List<Logs> logs = null;
//    string logsDBPath = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName, $"{DateTime.Today:ddMMyyyy}{ConfigProvider.Settings.LightDataExtension}");
//    if (Directory.Exists(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.LogsFolderName)) && File.Exists(logsDBPath))
//    {
//        using LogsContext logsContext = new(new Connection { Database = logsDBPath, DatabaseType = (byte)DatabaseType.Sqlite });
//        if (await logsContext.Logs.AsNoTracking().AnyAsync())
//        {
//            logs ??= new List<Logs>();
//            logs.AddRange(logsContext.Logs);
//        }
//    }
//    return logs;
//}