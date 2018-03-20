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
        public ProductRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
        }

        public override IEnumerable<Product> GetAll()
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(prod => prod.Name)
                .ToList();
        }

        public IEnumerable<Product> GetByCategoryId(Guid categoryId)
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(prod => prod.Name)
                .Where(prod => prod.CategoryId == categoryId)
                .ToList();
        }

        public IEnumerable<Product> GetRange(int startIndex, int offset)
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(prod => prod.Name)
                .Skip(startIndex - 1)
                .Take(offset)
                .ToList();
        }
    }
}