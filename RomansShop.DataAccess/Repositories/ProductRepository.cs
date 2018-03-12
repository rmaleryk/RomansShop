using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    internal class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ShopDbContext _shopDbContext;

        public ProductRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        public IEnumerable<Product> GetByCategoryId(Guid categoryId)
        {
            return _shopDbContext.Products
                .AsNoTracking()
                .Where(prod => prod.CategoryId == categoryId)
                .ToList();
        }

        public IEnumerable<Product> GetRange(int startIndex, int offset)
        {
            return _shopDbContext.Products
                .OrderBy(prod => prod.Name)
                .Skip(startIndex - 1)
                .Take(offset)
                .ToList();
        }
    }
}