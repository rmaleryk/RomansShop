using Autofac;
using RomansShop.Services;
using RomansShop.Services.Extensibility;

namespace RomansShop.WebApi
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductService>().As<IProductService>();
        }
    }
}