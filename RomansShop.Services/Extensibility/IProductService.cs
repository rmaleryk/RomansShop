using System;
using System.Collections.Generic;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;

namespace RomansShop.Services.Extensibility
{
    public interface IProductService
    {
        ValidationResponse<IEnumerable<Product>> GetRange(int startIndex, int offset);

        ValidationResponse<Product> GetById(Guid id);

        ValidationResponse<Product> Update(Product product);

        ValidationResponse<Product> Delete(Guid id);
    }
}