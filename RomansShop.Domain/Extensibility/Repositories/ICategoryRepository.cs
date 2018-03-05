using System;
using System.Collections.Generic;
using RomansShop.Domain.Entities;

namespace RomansShop.Domain.Extensibility.Repositories
{
    /// <summary>
    ///     Category Repository CRUD Interface
    /// </summary>
    public interface ICategoryRepository
    {
        Category Add(Category category);

        IEnumerable<Category> GetAll();

        Category GetById(Guid categoryId);

        Category GetByName(string categoryName);

        Category Update(Category category);

        void Delete(Category category);
    }
}