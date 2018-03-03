using System;
using System.Collections.Generic;
using Moq;
using RomansShop.Core;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services;
using RomansShop.Services.Extensibility;
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

        #region GetPage Tests

        [Fact]
        public void GetPage_ReturnsProducts_ForCorrectStartIndexAndOffset()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>();
            int startIndex = 1;
            int offset = 5;

            _mockRepository.Setup(repo => repo.GetPage(startIndex, offset)).Returns(products);

            // Act
            ValidationResponse<IEnumerable<Product>> actual = _productService.GetPage(startIndex, offset);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetPage_ReturnsStatusBadRequest_ForNegativeOrZeroStartIndexOrOffset()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>();
            int startIndex = -1;
            int offset = 0;

            // Act
            ValidationResponse<IEnumerable<Product>> actual = _productService.GetPage(startIndex, offset);

            // Assert
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_ReturnsProduct_ForExistsProductId()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            Product product = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);

            // Act
            ValidationResponse<Product> actual = _productService.GetById(productId);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetById_ReturnsNotFound_ForNonExistsProductId()
        {
            // Arrange
            Guid productId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(() => null);

            // Act
            ValidationResponse<Product> actual = _productService.GetById(productId);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_ReturnsProduct_ForExistProduct()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            Product product= new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);
            _mockRepository.Setup(repo => repo.Update(product)).Returns(product);

            // Act
            ValidationResponse<Product> actual = _productService.Update(product);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Update_ReturnsNotFound_ForNonExistProduct()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            Product category = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(() => null);

            // Act
            ValidationResponse<Product> actual = _productService.Update(category);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_ReturnsProduct_ForExistProduct()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            Product product = new Product() { Id = productId };

            _mockRepository.Setup(repo => repo.GetById(productId)).Returns(product);
            _mockRepository.Setup(repo => repo.Delete(product));

            // Act
            ValidationResponse<Product> actual = _productService.Delete(productId);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Delete_ReturnsNotFound_ForNonExistProduct()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            // Act
            ValidationResponse<Product> actual = _productService.Delete(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        #endregion

    }
}