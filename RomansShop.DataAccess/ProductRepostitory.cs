using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility;

namespace RomansShop.DataAccess
{
    /// <summary>
    ///     Entity Framework Implementation 
    ///     of Product Repository
    /// </summary>
    public class ProductRepostitory : IProductRepository
    {

        private readonly ShopDbContext _shopDbContext;

        public ProductRepostitory(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }


        public Product AddProduct(Product product)
        {
            _shopDbContext.Add(product);
            _shopDbContext.SaveChanges();

            return product;
        }

        public void DeleteProduct(int ProductId)
        {
            var product = _shopDbContext.Products.FirstOrDefault(p => p.Id == ProductId);

            if (product == null)
                return;

            _shopDbContext.Products.Remove(product);
            _shopDbContext.SaveChanges();
        }

        public Product GetProduct(int ProductId)
        {
            var product = _shopDbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == ProductId);

            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _shopDbContext.Products.ToList();
        }

        public void UpdateProduct(Product product)
        {
            var prod = _shopDbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);

            if (prod == null)
                return;

            _shopDbContext.Products.Update(product);
            _shopDbContext.SaveChanges();
        }
    }
}
