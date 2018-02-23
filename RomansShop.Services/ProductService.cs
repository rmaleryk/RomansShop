using System;
using System.Collections.Generic;
using System.Text;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility;
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

        public Product AddProduct(Product product)
        {
            return _productRepository.AddProduct(product);
        }

        public void DeleteProduct(int ProductId)
        {
            _productRepository.DeleteProduct(ProductId);
        }

        public Product GetProduct(int ProductId)
        {
            return _productRepository.GetProduct(ProductId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public void UpdateProduct(Product product)
        {
            UpdateProduct(product);
        }
    }
}
