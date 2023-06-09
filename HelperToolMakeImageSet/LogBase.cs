using System;
using System.Diagnostics;
using System.IO;

namespace HelperToolMakeImageSet
{
    public enum LogTarget
    {
        File, FileConsole, Database, EventLog, Console
    }
    
    public abstract class LogBase
    {
        protected readonly object lockObj = new object();
        public abstract void Log(string message);
    }

    public class FileLogger : LogBase
    {
        public string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public override void Log(string message)
        {
            lock (lockObj)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }
    
    public class FileConsoleLogger : LogBase
    {
        private string filePath;
        private readonly FileLogger _fileLogger;

        public FileConsoleLogger(string filePath)
        {
            this.filePath = filePath;
            _fileLogger = new FileLogger(filePath);
        }

        public override void Log(string message)
        {
            lock (lockObj)
            {
                _fileLogger.Log(message);
                Console.WriteLine(message);
            }
        }
    }
    
    public class ConsoleLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (lockObj)
            {
                Debug.WriteLine(message);
            }
        }
    }
    
    public static class LogHelper
    {
        private static LogBase logger = null;
        public static string PrefixMessage => DateTime.Now.ToString()+"|";
        public static void Log(LogTarget target, string message)
        {
            switch(target)
            {
                case LogTarget.File:
                    logger = new FileLogger(@"C:\Users\dmytro.dmytriiev\source\repos\HelperToolMakeImageSet_source_code\HelperToolMakeImageSet\Log.log");
                    logger.Log(PrefixMessage != "" | PrefixMessage != null ? PrefixMessage : "" + message);
                    break;
                case LogTarget.FileConsole:
                    logger = new FileConsoleLogger(@"C:\Users\dmytro.dmytriiev\source\repos\HelperToolMakeImageSet_source_code\HelperToolMakeImageSet\Log.log");
                    logger.Log(PrefixMessage != "" | PrefixMessage != null ? PrefixMessage : "" + message);
                    break;
                case LogTarget.Console:
                    logger = new ConsoleLogger();
                    logger.Log((PrefixMessage != "" | PrefixMessage != null ? PrefixMessage : "") + message);
                    break;
                default:
                    return;
            }
        }
    }
}