using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Logger
{
    public class Logger : ILogger
    {
        public Logger(Configuration configuration)
        {
            _loggers.Add(new ConsoleLogger() { Configuration = configuration });
            _loggers.Add(new FileLogger() { Configuration = configuration });
        }

        public Logger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public Logger(IEnumerable<ILogger> loggers)
        {
            _loggers.AddRange(loggers);
        }

        private readonly List<ILogger> _loggers = new List<ILogger>();

        public Configuration Configuration { get; set; }

        public void debug(string message)
        {
            _loggers.Where(log => log.Configuration.Debug.Any(value => value.Equals(log.GetName())))
                .AsParallel()
                .ForAll(log => log.debug(message));
        }

        public void error(string message)
        {
            _loggers.Where(log => log.Configuration.Error.Any(value => value.Equals(log.GetName())))
                .AsParallel()
                .ForAll(log => log.error(message));
        }

        public void error(Exception exception)
        {
            _loggers.Where(log => log.Configuration.Error.Any(value => value.Equals(log.GetName())))
                .AsParallel()
                .ForAll(log => log.error(exception));
        }

        public void info(string message)
        {
            _loggers.Where(log => log.Configuration.Info.Any(value => value.Equals(log.GetName())))
                .AsParallel()
                .ForAll(log => log.info(message));
        }

        public void warning(string message)
        {
            _loggers.Where(log => log.Configuration.Warning.Any(value => value.Equals(log.GetName())))
                .AsParallel()
                .ForAll(log => log.warning(message));
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
