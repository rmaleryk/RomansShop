using System;

namespace RomansShop.Core.Extensibility
{
    public interface ILogger
    {
        void Info(string message);

        void Error(string message);

        void Error(Exception exception, string message);
    }
}