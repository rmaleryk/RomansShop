using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ILoggerFactory = RomansShop.Core.Extensibility.ILoggerFactory;
using ILogger = RomansShop.Core.Extensibility.ILogger;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services;
using RomansShop.Tests.Common;
using Xunit;

namespace RomansShop.Tests.Services
{
    public class ProductServiceTests : UnitTestBase
    {
        private Mock<IProductRepository> _mockRepository;
        private Mock<ILogger> _mockLogger;

        private ProductService _productService;

        private static readonly Guid _productId = new Guid("00000000-0000-0000-0000-000000000001");

        public ProductServiceTests()
        {
            _mockRepository = MockRepository.Create<IProductRepository>();
            _mockLogger = MockRepository.Create<ILogger>();

            Mock<ILoggerFactory> mockLoggerFactory = MockRepository.Create<ILoggerFactory>();

            mockLoggerFactory
                .Setup(lf => lf.CreateLogger(typeof(ProductService)))
                .Returns(_mockLogger.Object);

            _productService = new ProductService(_mockRepository.Object, mockLoggerFactory.Object);
        }

        [Fact(DisplayName = "GetRange Products")]
        public void GetRangeTest()
        {
            IEnumerable<Product> products = new List<Product> { GetProduct(), GetProduct() };
            int startIndex = 1;
            int offset = 2;

            _mockRepository
                .Setup(repo => repo.GetRange(startIndex, offset))
                .Returns(products);

            ValidationResponse<IEnumerable<Product>> actual = _productService.GetRange(startIndex, offset);
            int actualCount = actual.ResponseData.Count();

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(products.Count(), actualCount);
        }

        [Fact(DisplayName = "GetById Product")]
        public void GetByIdTest()
        {
            Product product = GetProduct();

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(product);

            ValidationResponse<Product> actual = _productService.GetById(_productId);
            Guid actualId = actual.ResponseData.Id;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(product.Id, actualId);
        }

        [Fact(DisplayName = "GetById Product not found")]
        public void GetByIdProductNotFound()
        {
            string expectedMessage = $"Product with id {_productId} not found.";

            _mockLogger.Setup(logger => logger.Info(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(() => null);

            ValidationResponse<Product> actual = _productService.GetById(_productId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = "Update Product")]
        public void UpdateTest()
        {
            Product product = GetProduct(); 

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(product);

            _mockRepository
                .Setup(repo => repo.Update(product))
                .Returns(product);

            ValidationResponse<Product> actual = _productService.Update(product);
            string actualName = actual.ResponseData.Name;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(product.Name, actualName);
        }

        [Fact(DisplayName = "Update Product not found")]
        public void UpdateProductNotFound()
        {
            Product product = GetProduct();
            string expectedMessage = $"Product with id {_productId} not found.";

            _mockLogger.Setup(logger => logger.Info(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(() => null);

            ValidationResponse<Product> actual = _productService.Update(product);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = "Delete Product")]
        public void DeleteTest()
        {
            Product product = GetProduct();

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(product);

            _mockRepository.Setup(repo => repo.Delete(product));

            ValidationResponse<Product> actual = _productService.Delete(_productId);
            Guid actualId = actual.ResponseData.Id;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(product.Id, actualId);
        }

        [Fact(DisplayName = "Delete Product not found")]
        public void DeleteProductNotFound()
        {
            string expectedMessage = $"Product with id {_productId} not found.";

            _mockLogger.Setup(logger => logger.Info(expectedMessage));

            _mockRepository
                .Setup(repo => repo.GetById(_productId))
                .Returns(() => null);

            ValidationResponse<Product> actual = _productService.Delete(_productId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        private static Product GetProduct() =>
            new Product
            {
                Id = _productId,
                Name = "TestProduct"
            };
    }
}