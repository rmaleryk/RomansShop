using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    /// <summary>
    ///     Entity Framework Implementation 
    ///     of Product Repository
    /// </summary>
    public class ProductRepository : IProductRepository
    {

        private readonly ShopDbContext _shopDbContext;

        public ProductRepository(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        public Product Add(Product product)
        {
            _shopDbContext.Add(product);
            _shopDbContext.SaveChanges();

            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return _shopDbContext.Products.ToList();
        }

        public Product GetById(Guid productId)
        {
            return _shopDbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == productId);
        }

        public Product Update(Product product)
        {
            _shopDbContext.Products.Update(product);
            _shopDbContext.SaveChanges();

            return product;
        }

        public void Delete(Product product)
        {
            _shopDbContext.Products.Remove(product);
            _shopDbContext.SaveChanges();
        }

        public IEnumerable<Product> GetByCategoryId(Guid categoryId)
        {
            return _shopDbContext.Products.Where(prod => prod.CategoryId == categoryId);
        }

        public IEnumerable<Product> GetPage(int startIndex, int endIndex)
        {
            // TODO: Warning "uses a row limiting operation (Skip/Take) without OrderBy which may lead to unpredictable results".
            // 1) Add the date of addition in the DB and sort by it.
            // 2) Don't worry!

            return _shopDbContext.Products
                        .Skip(startIndex - 1)
                        .Take(endIndex - (startIndex - 1));
        }
    }
}