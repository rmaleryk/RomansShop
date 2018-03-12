using System;

namespace RomansShop.Core.Extensibility.Logger
{
    public interface ILogger
    {
        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogError(Exception exception, string message);
    }
}