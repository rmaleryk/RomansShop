using System;
using System.Collections.Generic;
using RomansShop.Core;
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

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ValidationResponse<IEnumerable<Product>> GetPage(int startIndex, int offset)
        {
            if (startIndex <= 0 || offset <= 0)
            {
                return new ValidationResponse<IEnumerable<Product>>()
                {
                    Status = ValidationStatus.Failed,
                    Message = "The start index or offset is incorrect."
                };
            }

            IEnumerable<Product> products = _productRepository.GetPage(startIndex, offset);

            return new ValidationResponse<IEnumerable<Product>>() { ResponseData = products };
        }

        public ValidationResponse<Product> GetById(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return new ValidationResponse<Product>()
                {
                    Status = ValidationStatus.NotFound,
                    Message = "Product not found."
                };
            }

            return new ValidationResponse<Product>() { ResponseData = product };
        }

        public ValidationResponse<Product> Update(Product product)
        {
            Product productTmp = _productRepository.GetById(product.Id);

            if (productTmp == null)
            {
                return new ValidationResponse<Product>()
                {
                    Status = ValidationStatus.NotFound,
                    Message = "Product not found."
                };
            }

            product = _productRepository.Update(product);

            return new ValidationResponse<Product>() { ResponseData = product };
        }

        public ValidationResponse<Product> Delete(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                return new ValidationResponse<Product>()
                {
                    Status = ValidationStatus.NotFound,
                    Message = "Product not found."
                };
            }

            _productRepository.Delete(product);

            return new ValidationResponse<Product>()
            {
                ResponseData = product,
                Message = "Product was deleted."
            };
        }
    }
}