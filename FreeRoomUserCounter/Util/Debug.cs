using System;
using System.IO;

namespace MonitoringBot.Util
{
    static class Debug
    {
        private static readonly string LogFileName = $"debug.log";
        private static readonly string LogFileDir = $"{Directory.GetCurrentDirectory()}/data";

        static Debug()
        {
            Directory.CreateDirectory(LogFileDir);
        }
        public static void Log(object obj)
        {
            Console.WriteLine($"{DateTime.Now}:{obj.ToString()}");
            using (var writer = File.AppendText($"{LogFileDir}/{LogFileName}"))
            {
                writer.WriteLine($"{DateTime.Now}:{obj.ToString()}");
            }
        }
    }
}
