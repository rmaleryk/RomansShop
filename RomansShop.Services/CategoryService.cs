using System;
using System.Collections.Generic;
using System.Linq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;

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
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Add(Category category)
        {
            Category categoryTmp = _categoryRepository.GetByName(category.Name);

            if (categoryTmp != null)
            {
                string message = $"Category name \"{category.Name}\" already exist.";
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            category = _categoryRepository.Add(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Update(Category category)
        {
            Category categoryTmp = _categoryRepository.GetById(category.Id);

            if (categoryTmp == null)
            {
                string message = $"Category with id {category.Id} not found.";
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            categoryTmp = _categoryRepository.GetByName(category.Name);

            if (categoryTmp != null && categoryTmp.Id != category.Id)
            {
                string message = $"Category name \"{category.Name}\" already exist.";
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            category = _categoryRepository.Update(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok);
        }

        public ValidationResponse<Category> Delete(Guid id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category == null)
            {
                string message = $"Category with id {id} not found.";
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.NotFound, message);
            }

            IEnumerable<Product> products = _productRepository.GetByCategoryId(category.Id);

            if (products.Any())
            {
                string message = $"Category with id {id} is not empty.";
                _logger.Info(message);

                return new ValidationResponse<Category>(ValidationStatus.Failed, message);
            }

            _categoryRepository.Delete(category);

            return new ValidationResponse<Category>(category, ValidationStatus.Ok, 
                $"Category with id {id} was deleted.");
        }
    }
}