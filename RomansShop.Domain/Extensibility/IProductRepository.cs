using System;
using System.Collections.Generic;
using System.Text;

namespace RomansShop.Domain.Extensibility
{
    /// <summary>
    ///     Repository CRUD Interface
    /// </summary>
    public interface IProductRepository
    {
        Product AddProduct(Product product);

        IEnumerable<Product> GetProducts();

        Product GetProduct(int ProductId);

        void UpdateProduct(Product product);

        void DeleteProduct(int ProductId);
    }
}
