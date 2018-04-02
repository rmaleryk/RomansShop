using System;
using System.Collections.Generic;
using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetByCategoryId(Guid categoryId);
        
        IEnumerable<Product> SearchByName(string productName);

        IEnumerable<Product> GetRange(int startIndex, int offset);
    }
}