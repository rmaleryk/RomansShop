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
    }
}