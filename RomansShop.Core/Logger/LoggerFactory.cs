using System;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;

namespace RomansShop.Core.Logger
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new NLogger(type);
        }
    }
}