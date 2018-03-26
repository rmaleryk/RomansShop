using Autofac;
using RomansShop.DataAccess.Repositories;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
        }
    }
}