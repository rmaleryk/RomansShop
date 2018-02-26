using Autofac;
using RomansShop.DataAccess.Repositories;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.WebApi
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
        }
    }
}