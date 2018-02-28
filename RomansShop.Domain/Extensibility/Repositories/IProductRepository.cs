using System;
using System.Collections.Generic;

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
    }
}