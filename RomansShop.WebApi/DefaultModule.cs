using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using RomansShop.DataAccess;
using RomansShop.Domain.Extensibility;
using RomansShop.Services;
using RomansShop.Services.Extensibility;

namespace RomansShop.WebApi
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepostitory>().As<IProductRepository>();
            builder.RegisterType<ProductService>().As<IProductService>();
        }
    }
}
