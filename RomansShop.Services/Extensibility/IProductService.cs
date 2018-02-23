using System;
using System.Collections.Generic;
using System.Text;
using RomansShop.Domain;

namespace RomansShop.Services.Extensibility
{
    public interface IProductService
    {
        Product AddProduct(Product product);

        IEnumerable<Product> GetProducts();

        Product GetProduct(int ProductId);

        void UpdateProduct(Product product);

        void DeleteProduct(int ProductId);
    }
}
