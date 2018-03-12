using System;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;

namespace RomansShop.Core.Logger
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new Logger(type);
        }
    }
}