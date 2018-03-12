using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ShopDbContext _shopDbContext;

        public CategoryRepository(ShopDbContext shopDbContext) : base(shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        public Category GetByName(string categoryName)
        {
            return _shopDbContext.Categories.AsNoTracking().FirstOrDefault(cat => cat.Name == categoryName);
        }
    }
}