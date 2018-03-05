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

        [Fact]
        public void GetById_ReturnsCategory_ForExistsCategoryId()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);

            ValidationResponse<Category> actual = _categoryService.GetById(categoryId);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void GetById_ReturnsNotFound_ForNonExistsCategoryId()
        {
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.GetById(categoryId);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Add_ReturnsCategory_ForUniqueCategoryName()
        {
            Category category = new Category();

            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(() => null);
            _mockRepository.Setup(repo => repo.Add(category)).Returns(category);

            ValidationResponse<Category> actual = _categoryService.Add(category);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Add_ReturnsFailed_ForNonUniqueCategoryName()
        {
            Category category = new Category();

            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(new Category());

            ValidationResponse<Category> actual = _categoryService.Add(category);

            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact]
        public void Update_ReturnsCategory_ForExistCategoryWithUniqueName()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(() => null);
            _mockRepository.Setup(repo => repo.Update(category)).Returns(category);

            ValidationResponse<Category> actual = _categoryService.Update(category);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Update_ReturnsNotFound_ForNonExistCategory()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.Update(category);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Update_ReturnsFailed_ForNonUniqueCategoryName()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockRepository.Setup(repo => repo.GetByName(category.Name)).Returns(new Category());

            ValidationResponse<Category> actual = _categoryService.Update(category);

            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact]
        public void Delete_ReturnsCategory_ForExistEmptyCategory()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(category);
            _mockProductRepository.Setup(repo => repo.GetByCategoryId(category.Id)).Returns(new List<Product>());
            _mockRepository.Setup(repo => repo.Delete(category));

            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.NotNull(actual.ResponseData);
        }

        [Fact]
        public void Delete_ReturnsNotFound_ForNonExistCategory()
        {
            Guid categoryId = Guid.NewGuid();

            _mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact]
        public void Delete_ReturnsFailed_ForExistNonEmptyCategory()
        {
            Guid categoryId = Guid.NewGuid();
            Category category = new Category() { Id = categoryId };

            _mockRepository.Setup(repo => repo.GetById(category.Id)).Returns(category);
            _mockProductRepository.Setup(repo => repo.GetByCategoryId(category.Id)).Returns(new List<Product>() { new Product() });

            ValidationResponse<Category> actual = _categoryService.Delete(categoryId);

            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }
    }
}
