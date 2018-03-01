using System;
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
        private ICategoryService _categoryService { get; set; }

        public CategoryServiceTests()
        {
            _mockRepository = MockRepository.Create<ICategoryRepository>();

            _categoryService = new CategoryService(_mockRepository.Object);
        }

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
    }
}
