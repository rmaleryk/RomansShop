using System;
using System.Collections.Generic;
using System.Linq;
using RomansShop.Domain;
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

        public bool IsExist(Guid id)
        {
            return _categoryRepository.GetById(id) != null;
        }

        public bool isEmpty(Guid id)
        {
            IEnumerable<Product> products = _productRepository.GetByCategoryId(id);

            if(products == null)
            {
                // TODO: Is it right?
                throw new Exception("Category not found!"); 
            }

            if (products.Count() != 0)
            {
                return false;
            }

            return true;
        }
    }
}
