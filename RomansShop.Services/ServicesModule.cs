﻿using Autofac;
using RomansShop.Services.Extensibility;

namespace RomansShop.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<OrderService>().As<IOrderService>();
        }
    }
}