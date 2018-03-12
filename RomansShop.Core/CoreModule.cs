using Autofac;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using LoggerFactory = RomansShop.Core.Logger.LoggerFactory;

namespace RomansShop.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
        }
    }
}