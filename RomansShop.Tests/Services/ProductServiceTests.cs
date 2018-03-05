using System;
using System.Collections.Generic;
using Moq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services;
using RomansShop.Services.Extensibility;
using RomansShop.Tests.Common;
using Xunit;

namespace RomansShop.Tests.Services
{
    public class ProductServiceTests : UnitTestBase
    {
        private Mock<IProductRepository> _mockRepository { get; set; }
        private IProductService _productService { get; set; }

        public ProductServiceTests()
        {
            _mockRepository = MockRepository.Create<IProductRepository>();

            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact]
        public void GetPage_ReturnsProducts_ForCorrectStartIndexAndOffset()
        {
            IEnumerable<Product> products = new List<Product>();
            int startIndex = 1;
            int offset = 5;

            _mockRepository.Setup(repo => repo.GetRange(startIndex, offset)).Returns(products);

            ValidationResponse<IEnumerable<Product>> actual = _productService.GetRange(startIndex, offset);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetPage_ReturnsStatusBadRequest_ForNegativeOrZeroStartIndexOrOffset()
        {
            IEnumerable<Product> products = new List<Product>();
            int startIndex = -1;
            int offset = 0;

            ValidationResponse<IEnumerable<Product>> actual = _productService.GetRange(startIndex, offset);

            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact]
        public void GetById_ReturnsProduct_ForExistsProductId()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);

            ValidationResponse<Product> actual = _productService.GetById(productId);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetById_ReturnsNotFound_ForNonExistsProductId()
        {
            Guid productId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(() => null);

            ValidationResponse<Product> actual = _productService.GetById(productId);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Update_ReturnsProduct_ForExistProduct()
        {
            Guid productId = Guid.NewGuid();
            Product product= new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);
            _mockRepository.Setup(repo => repo.Update(product)).Returns(product);

            ValidationResponse<Product> actual = _productService.Update(product);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Update_ReturnsNotFound_ForNonExistProduct()
        {
            Guid productId = Guid.NewGuid();
            Product category = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(() => null);

            ValidationResponse<Product> actual = _productService.Update(category);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Delete_ReturnsProduct_ForExistProduct()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);
            _mockRepository.Setup(repo => repo.Delete(product));

            ValidationResponse<Product> actual = _productService.Delete(productId);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Delete_ReturnsNotFound_ForNonExistProduct()
        {
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            ValidationResponse<Product> actual = _productService.Delete(categoryId);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }
    }
}