using System;
using System.Collections.Generic;
using System.Linq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;

namespace RomansShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public ValidationResponse<Category> GetById(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return new ValidationResponse<Category>(ValidationStatus.NotFound, "Category not found.");
            }

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Add(Category category)
        {
            Category categoryTmp = _categoryRepository.GetByName(category.Name);

            if (categoryTmp != null)
            {
                return new ValidationResponse<Category>(ValidationStatus.Failed, "Category name already exist.");
            }

            category = _categoryRepository.Add(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Update(Category category)
        {
            // Check the category existence
            Category categoryTmp = _categoryRepository.GetById(category.Id);

            if (categoryTmp == null)
            {
                return new ValidationResponse<Category>(ValidationStatus.NotFound, "Category not found.");
            }

            // Check name duplication (skip the same id)
            categoryTmp = _categoryRepository.GetByName(category.Name);

            if (categoryTmp != null && categoryTmp.Id != category.Id)
            {
                return new ValidationResponse<Category>(ValidationStatus.Failed, "Category name already exist.");
            }

            category = _categoryRepository.Update(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Delete(Guid id)
        {
            // Check the category existence
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                return new ValidationResponse<Category>(ValidationStatus.NotFound, "Category not found.");
            }

            // Check category content (only empty category is available to delete)
            IEnumerable<Product> products = _productRepository.GetByCategoryId(category.Id);

            if (products.Count() != 0)
            {
                return new ValidationResponse<Category>(ValidationStatus.Failed, "Category is not empty.");
            }

            _categoryRepository.Delete(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok, "Category was deleted.");
        }
    }
}