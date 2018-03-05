using System;
using System.Collections.Generic;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
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

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ValidationResponse<IEnumerable<Product>> GetRange(int startIndex, int offset)
        {
            if (startIndex <= 0 || offset <= 0)
            {
                return new ValidationResponse<IEnumerable<Product>>(ValidationStatus.Failed, "The start index or offset is incorrect.");
            }

            IEnumerable<Product> products = _productRepository.GetRange(startIndex, offset);

            return new ValidationResponse<IEnumerable<Product>>(products, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> GetById(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return new ValidationResponse<Product>(ValidationStatus.NotFound, "Product not found.");
            }

            return new ValidationResponse<Product>(product, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> Update(Product product)
        {
            Product productTmp = _productRepository.GetById(product.Id);

            if (productTmp == null)
            {
                return new ValidationResponse<Product>(ValidationStatus.NotFound, "Product not found.");
            }

            product = _productRepository.Update(product);

            return new ValidationResponse<Product>(product, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> Delete(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return new ValidationResponse<Product>(ValidationStatus.NotFound, "Product not found.");
            }

            _productRepository.Delete(product);

            return new ValidationResponse<Product>(product, ValidationStatus.Ok, "Product was deleted.");
        }
    }
}