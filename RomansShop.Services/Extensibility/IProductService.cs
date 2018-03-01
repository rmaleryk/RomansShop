using System;
using System.Collections.Generic;
using RomansShop.Domain;

namespace RomansShop.Services.Extensibility
{
    public interface IProductService
    {
        IEnumerable<Product> GetByCategoryId(Guid categoryId);
    }
}