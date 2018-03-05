using System;
using System.Collections.Generic;
using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    /// <summary>
    ///     Repository CRUD Interface
    /// </summary>
    public interface IProductRepository
    {
        Product Add(Product product);

        IEnumerable<Product> GetAll();

        Product GetById(Guid productId);

        Product Update(Product product);

        void Delete(Product product);

        IEnumerable<Product> GetByCategoryId(Guid categoryId);

        IEnumerable<Product> GetRange(int startIndex, int offset);
    }
}