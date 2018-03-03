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

        #region GetById Tests

        [Fact]
        public void GetById_ReturnsCategory_ForExistsCategoryId()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

            // Act
            ValidationResponse<Category> actual = _categoryService.GetById(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetById_ReturnsNotFound_ForNonExistsCategoryId()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            // Act
            ValidationResponse<Category> actual = _categoryService.GetById(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        #endregion

        #region Add Tests

        [Fact]
        public void Add_ReturnsCategory_ForUniqueCategoryName()
        {
            // Arrange
            Category category = new Category();

            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(() => null);
            _mockRepository.Setup(repo => repo.Add(category)).Returns(category);

            // Act
            ValidationResponse<Category> actual = _categoryService.Add(category);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Add_ReturnsFailed_ForNonUniqueCategoryName()
        {
            // Arrange
            Category category = new Category();

            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(new Category());

            // Act
            ValidationResponse<Category> actual = _categoryService.Add(category);

            // Assert
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_ReturnsCategory_ForExistCategoryWithUniqueName()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(() => null);
            _mockRepository.Setup(repo => repo.Update(category)).Returns(category);

            // Act
            ValidationResponse<Category> actual = _categoryService.Update(category);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Update_ReturnsNotFound_ForNonExistCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            // Act
            ValidationResponse<Category> actual = _categoryService.Update(category);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Update_ReturnsFailed_ForNonUniqueCategoryName()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(new Category());

            // Act
            ValidationResponse<Category> actual = _categoryService.Update(category);

            // Assert
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_ReturnsCategory_ForExistEmptyCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockProductRepository.Setup(repo => repo.GetByCategoryId(category.Id)).Returns(new List<Product>());
            _mockRepository.Setup(repo => repo.Delete(category));

            // Act
            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Delete_ReturnsNotFound_ForNonExistCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            // Act
            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Delete_ReturnsFailed_ForExistNonEmptyCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(category.Id)).Returns(category);
            _mockProductRepository.Setup(repo => repo.GetByCategoryId(category.Id)).Returns(new List<Product>() { new Product() });

            // Act
            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            // Assert
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        #endregion
    }
}
