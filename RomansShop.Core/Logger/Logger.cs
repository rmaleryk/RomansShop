using System;
using NLog;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;

namespace RomansShop.Core.Logger
{
    public class Logger : ILogger
    {
        private readonly NLog.ILogger _logger;

        public Logger(Type type)
        {
            _logger = LogManager.GetLogger(type.Name);
        }

        public void LogInfo(string message) => _logger.Info(message);

        public void LogWarning(string message) => _logger.Warn(message);

        public void LogError(string message) => _logger.Error(message);

        public void LogError(Exception exception, string message) => _logger.Error(exception, message);
    }
}