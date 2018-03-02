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
    public class CategoryServiceTests : UnitTestBase
    {
        private Mock<ICategoryRepository> _mockRepository { get; set; }
        private Mock<IProductRepository> _mockProductRepository { get; set; }
        private ICategoryService _categoryService { get; set; }

        public CategoryServiceTests()
        {
            _mockRepository = MockRepository.Create<ICategoryRepository>();
            _mockProductRepository = MockRepository.Create<IProductRepository>();

            _categoryService = new CategoryService(_mockRepository.Object, _mockProductRepository.Object);
        }

        #region IsExist Tests

        [Fact]
        public void IsExist_ReturnsTrue_ForExistsCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

            // Act
            bool actual = _categoryService.IsExist(categoryId);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsExist_ReturnsFalse_ForNonExistsCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            // Act
            bool actual = _categoryService.IsExist(categoryId);

            // Assert
            Assert.False(actual);
        }

        #endregion

        #region isEmpty Tests

         [Fact]
        public void IsEmpty_ReturnsTrue_ForEmptyCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> emptyProducts = new List<Product>();

            _mockProductRepository.Setup(serv => serv.GetByCategoryId(categoryId)).Returns(emptyProducts);

            // Act
            bool actual = _categoryService.isEmpty(categoryId);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsEmpty_ReturnsFalse_ForNonEmptyCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>() { new Product() };

            _mockProductRepository.Setup(serv => serv.GetByCategoryId(categoryId)).Returns(products);

            // Act
            bool actual = _categoryService.isEmpty(categoryId);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsEmpty_ThrowsException_ForNonExistCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockProductRepository.Setup(serv => serv.GetByCategoryId(categoryId)).Returns(() => null);

            // Act
            Action actual = () => _categoryService.isEmpty(categoryId);

            // Assert
            Assert.Throws<Exception>(actual);
        }

        #endregion

    }
}
