using System;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;

namespace RomansShop.Services.Extensibility
{
    public interface ICategoryService
    {
        ValidationResponse<Category> GetById(Guid id);

        ValidationResponse<Category> Add(Category category);

        ValidationResponse<Category> Update(Category category);

        ValidationResponse<Category> Delete(Guid id);
    }
}