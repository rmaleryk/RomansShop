using System;
using System.Collections.Generic;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;

namespace RomansShop.Services
{
    /// <summary>
    ///     Product Service
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
        }

        public IEnumerable<Product> GetByCategoryId(Guid categoryId)
        {
            if (!_categoryService.IsExist(categoryId))
            {
                return null;
            }

            return _productRepository.GetByCategoryId(categoryId);
        }
    }
}