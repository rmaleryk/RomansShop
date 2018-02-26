using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using RomansShop.DataAccess;
using RomansShop.Domain.Extensibility;

namespace RomansShop.WebApi
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepostitory>().As<IProductRepository>();
        }
    }
}
