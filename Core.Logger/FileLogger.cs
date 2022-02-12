using System;
using System.IO;

namespace Core.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _directory = $"{Directory.GetCurrentDirectory()}/Log/";

        public Configuration Configuration { get; set; }

        private void CreateDirectory()
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        private void Write(string file, string message)
        {
            file = _directory + file;
            CreateDirectory();

            if (!File.Exists(file))
            {
                File.WriteAllText(file, message);
                return;
            }

            File.AppendAllText(file, message);
        }

        private void Write(ILogger.Type type, string message)
        {
            var fileLogType = $"{type.ToString().ToLower()}_{DateTime.Today:dd-MM-yyyy}.log";
            var fileLogAll = $"log_{DateTime.Today:dd-MM-yyyy}.log";

            message = $"[{DateTime.Now:G}] [{type}] {message}\n";
            Write(fileLogType, message);
            Write(fileLogAll, message);
        }

        public void debug(string message)
        {
            Write(ILogger.Type.DEBUG, message);
        }

        public void error(string message)
        {
            Write(ILogger.Type.ERROR, message);
        }

        public void error(Exception exception)
        {
            error($"{exception.Message}\n{exception.StackTrace}");
        }

        public void info(string message)
        {
            Write(ILogger.Type.INFO, message);
        }

        public void warning(string message)
        {
            Write(ILogger.Type.WARNING, message);
        }

        public string GetName()
        {
            return "File";
        }
    }
}
