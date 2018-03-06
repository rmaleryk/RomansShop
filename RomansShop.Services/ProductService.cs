using System;
using System.Collections.Generic;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;

namespace RomansShop.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public ProductService(IProductRepository productRepository, ILoggerFactory loggerFactory)
        {
            _productRepository = productRepository;
            _loggerFactory = loggerFactory;

            _logger = _loggerFactory.CreateLogger(GetType());
        }

        public ValidationResponse<IEnumerable<Product>> GetRange(int startIndex, int offset)
        {
            IEnumerable<Product> products = _productRepository.GetRange(startIndex, offset);

            return new ValidationResponse<IEnumerable<Product>>(products, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> GetById(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                string message = $"Product with id {id} not found.";
                _logger.Info(message);

                return new ValidationResponse<Product>(ValidationStatus.NotFound, message);
            }

            return new ValidationResponse<Product>(product, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> Update(Product product)
        {
            Product productTmp = _productRepository.GetById(product.Id);

            if (productTmp == null)
            {
                string message = $"Product with id {product.Id} not found.";
                _logger.Info(message);

                return new ValidationResponse<Product>(ValidationStatus.NotFound, message);
            }

            product = _productRepository.Update(product);

            return new ValidationResponse<Product>(product, ValidationStatus.Ok);
        }

        public ValidationResponse<Product> Delete(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (product == null)
            {
                string message = $"Product with id {id} not found.";
                _logger.Info(message);

                return new ValidationResponse<Product>(ValidationStatus.NotFound, message);
            }

            _productRepository.Delete(product);

            return new ValidationResponse<Product>(product, ValidationStatus.Ok, 
                $"Product with id {id} was deleted.");
        }
    }
}