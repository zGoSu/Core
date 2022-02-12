using System;

namespace Core.Logger
{
    public class ConsoleLogger : ILogger
    {
        public Configuration Configuration { get; set; }

        private void SendMessage(string message, ILogger.Type type)
        {
            Console.WriteLine($"[{DateTime.Now:G}] [{type}] {message}");
        }

        public void debug(string message)
        {
            SendMessage(message, ILogger.Type.DEBUG);
        }

        public void error(string message)
        {
            SendMessage(message, ILogger.Type.ERROR);
        }

        public void error(Exception exception)
        {
            error($"{exception.Message}\n{exception.StackTrace}");
        }

        public void info(string message)
        {
            SendMessage(message, ILogger.Type.INFO);
        }

        public void warning(string message)
        {
            SendMessage(message, ILogger.Type.WARNING);
        }

        public string GetName()
        {
            return "Console";
        }
    }
}
