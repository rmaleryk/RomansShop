using System;
using System.Collections.Generic;
using Moq;
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
        private Mock<ICategoryService> _mockCategoryService { get; set; }
        private IProductService _productService { get; set; }


        public ProductServiceTests()
        {
            _mockRepository = MockRepository.Create<IProductRepository>();
            _mockCategoryService = MockRepository.Create<ICategoryService>();

            _productService = new ProductService(_mockRepository.Object, _mockCategoryService.Object);
        }

        [Fact]
        public void GetByCategoryId_ReturnsProductList_ForExistsCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>();

            _mockCategoryService.Setup(serv => serv.IsExist(categoryId)).Returns(true);
            _mockRepository.Setup(repo => repo.GetByCategoryId(categoryId)).Returns(products);

            // Act
            IEnumerable<Product> actual = _productService.GetByCategoryId(categoryId);

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetByCategoryId_ReturnsNull_ForNonExistsCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockCategoryService.Setup(serv => serv.IsExist(categoryId)).Returns(false);

            // Act
            IEnumerable<Product> actual = _productService.GetByCategoryId(categoryId);

            // Assert
            Assert.Null(actual);
        }
    }
}