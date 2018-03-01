using System;
using System.Collections.Generic;

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

        Category Update(Category category);

        void Delete(Category category);
    }
}