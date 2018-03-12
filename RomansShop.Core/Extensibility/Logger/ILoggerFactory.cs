using System;

namespace RomansShop.Core.Extensibility.Logger
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}