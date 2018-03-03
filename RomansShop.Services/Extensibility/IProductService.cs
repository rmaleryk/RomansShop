using System;
using System.Collections.Generic;
using RomansShop.Core;
using RomansShop.Domain;

namespace RomansShop.Services.Extensibility
{
    public interface IProductService
    {
        ValidationResponse<IEnumerable<Product>> GetPage(int startIndex, int offset);

        ValidationResponse<Product> GetById(Guid id);

        ValidationResponse<Product> Update(Product product);

        ValidationResponse<Product> Delete(Guid id);
    }
}