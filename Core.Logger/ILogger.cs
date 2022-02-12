using System;

namespace Core.Logger
{
    public interface ILogger
    {
        public enum Type
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG
        };

        public Configuration Configuration { get; set; }
        public string GetName();
        public void info(string message);
        public void error(string message);
        public void error(Exception exception);
        public void warning(string message);
        public void debug(string message);
    }
}
