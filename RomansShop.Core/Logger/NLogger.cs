using System;
using NLog;
using ILogger = RomansShop.Core.Extensibility.ILogger;

namespace RomansShop.Core.Logger
{
    public class NLogger : ILogger
    {
        private readonly NLog.ILogger _logger;

        public NLogger(Type type)
        {
            _logger = LogManager.GetLogger(type.Name);
        }

        public void Info(string message) => _logger.Info(message);

        public void Error(string message) => _logger.Error(message);

        public void Error(Exception exception, string message) => _logger.Error(exception, message);
    }
}