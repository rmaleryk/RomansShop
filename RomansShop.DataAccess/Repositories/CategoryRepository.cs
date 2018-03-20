using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
        }

        public override IEnumerable<Category> GetAll()
        {
            return dbSet
                .AsNoTracking()
                .OrderBy(cat => cat.Name)
                .ToList();
        }

        public Category GetByName(string categoryName)
        {
            return dbSet
                .AsNoTracking()
                .FirstOrDefault(cat => cat.Name == categoryName);
        }
    }
}