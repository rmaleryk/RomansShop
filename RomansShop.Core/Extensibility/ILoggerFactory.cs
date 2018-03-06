using System;
using RomansShop.Core.Logger;

namespace RomansShop.Core.Extensibility
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}