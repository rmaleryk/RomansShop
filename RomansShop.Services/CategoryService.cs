using System;
using System.Collections.Generic;
using System.Linq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using ILoggerFactory = RomansShop.Core.Extensibility.Logger.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.Logger.ILogger;

namespace RomansShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, ILoggerFactory loggerFactory)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _loggerFactory = loggerFactory;

            _logger = _loggerFactory.CreateLogger(GetType());
        }

        public ValidationResponse<Category> GetById(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                string message = $"Category with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Add(Category category)
        {
            if (!IsUniqueName(category.Name))
            {
                string message = $"Category name \"{category.Name}\" already exist.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            Category addedCategory = _categoryRepository.Add(category);

            return new ValidationResponse<Category>(addedCategory, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Update(Category category)
        {
            Category categoryTmp = _categoryRepository.GetById(category.Id);

            if (categoryTmp == null)
            {
                string message = $"Category with id {category.Id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            categoryTmp = _categoryRepository.GetByName(category.Name);

            if (categoryTmp != null && categoryTmp.Id != category.Id)
            {
                string message = $"Category name \"{category.Name}\" already exist.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            Category updatedCategory = _categoryRepository.Update(category);

            return new ValidationResponse<Category>(updatedCategory, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Delete(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                string message = $"Category with id {id} not found.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            IEnumerable<Product> products = _productRepository.GetByCategoryId(category.Id);

            if (products.Any())
            {
                string message = $"Category with id {id} is not empty.";
                _logger.LogWarning(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            _categoryRepository.Delete(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok,
                $"Category with id {id} was deleted.");
        }

        private bool IsUniqueName(string name)
        {
            Category category = _categoryRepository.GetByName(name);

            if (category == null)
            {
                return true;
            }

            return false;
        }
    }
}