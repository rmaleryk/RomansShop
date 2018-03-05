using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RomansShop.DataAccess.Database;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;

namespace RomansShop.DataAccess.Repositories
{
    /// <summary>
    ///     Entity Framework Implementation 
    ///     of Category Repository
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ShopDbContext _shopDbContext;

        public CategoryRepository(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        public Category Add(Category category)
        {
            _shopDbContext.Add(category);
            _shopDbContext.SaveChanges();

            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return _shopDbContext.Categories.ToList();
        }

        public Category GetById(Guid categoryId)
        {
            return _shopDbContext.Categories.AsNoTracking().FirstOrDefault(cat => cat.Id == categoryId);
        }

        public Category GetByName(string categoryName)
        {
            return _shopDbContext.Categories.AsNoTracking().FirstOrDefault(cat => cat.Name == categoryName);
        }

        public Category Update(Category category)
        {
            _shopDbContext.Categories.Update(category);
            _shopDbContext.SaveChanges();

            return category;
        }

        public void Delete(Category category)
        {
            _shopDbContext.Categories.Remove(category);
            _shopDbContext.SaveChanges();
        }
    }
}