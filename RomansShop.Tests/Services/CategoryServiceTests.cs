using System;
using System.Collections.Generic;
using Moq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services;
using RomansShop.Tests.Common;
using Xunit;

namespace RomansShop.Tests.Services
{
    public class CategoryServiceTests : UnitTestBase
    {
        private Mock<ICategoryRepository> _mockRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private CategoryService _categoryService;

        private static readonly Guid _categoryId = new Guid("00000000-0000-0000-0000-000000000001");

        public CategoryServiceTests()
        {
            _mockRepository = MockRepository.Create<ICategoryRepository>();
            _mockProductRepository = MockRepository.Create<IProductRepository>();

            _categoryService = new CategoryService(_mockRepository.Object, _mockProductRepository.Object);
        }

        [Fact(DisplayName = "GetById Category")]
        public void GetByIdTest()
        {
            Category category = GetCategory();

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(category);

            ValidationResponse<Category> actual = _categoryService.GetById(_categoryId);
            Guid actualId = actual.ResponseData.Id;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(_categoryId, actualId);
        }

        [Fact(DisplayName = "GetById Category not found")]
        public void GetByIdCategoryNotFoundTest()
        {
            string expectedMessage = $"Category with id {_categoryId} not found.";

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.GetById(_categoryId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = "Add Category")]
        public void AddTest()
        {
            Category category = GetCategory();

            _mockRepository
                .Setup(repo => repo.GetByName(category.Name))
                .Returns(() => null);

            _mockRepository
                .Setup(repo => repo.Add(category))
                .Returns(category);

            ValidationResponse<Category> actual = _categoryService.Add(category);
            string actualName = actual.ResponseData.Name;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(category.Name, actualName);
        }

        [Fact(DisplayName = "Add Duplicate category")]
        public void AddDuplicateCategoryTest()
        {
            Category category = GetCategory();
            Category duplicateCategory = new Category { Name = category.Name };

            string expectedMessage = $"Category name \"{category.Name}\" already exist.";

            _mockRepository
                .Setup(repo => repo.GetByName(category.Name))
                .Returns(duplicateCategory);

            ValidationResponse<Category> actual = _categoryService.Add(category);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact(DisplayName = "Update Category")]
        public void UpdateTest()
        {
            Category category = GetCategory();

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(category);

            _mockRepository
                .Setup(repo => repo.GetByName(category.Name))
                .Returns(() => null);

            _mockRepository
                .Setup(repo => repo.Update(category))
                .Returns(category);

            ValidationResponse<Category> actual = _categoryService.Update(category);
            string actualName = actual.ResponseData.Name;

            Assert.Equal(ValidationStatus.Ok, actual.Status);
            Assert.Equal(category.Name, actualName);
        }

        [Fact(DisplayName = "Update Category not found")]
        public void UpdateCategoryNotFoundTest()
        {
            Category category = GetCategory();

            string expectedMessage = $"Category with id {_categoryId} not found.";

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.Update(category);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = "Update Duplicate category")]
        public void UpdateDuplicateCategoryTest()
        {
            Category category = GetCategory();
            Category duplicateCategory = new Category { Name = category.Name };

            string expectedMessage = $"Category name \"{category.Name}\" already exist.";

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(category);

            _mockRepository
                .Setup(repo => repo.GetByName(category.Name))
                .Returns(duplicateCategory);

            ValidationResponse<Category> actual = _categoryService.Update(category);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        [Fact(DisplayName = "Delete Category")]
        public void DeleteTest()
        {
            Category category = GetCategory();
            List<Product> emptyProductsList = new List<Product>();

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(category);

            _mockProductRepository
                .Setup(repo => repo.GetByCategoryId(category.Id))
                .Returns(emptyProductsList);

            _mockRepository.Setup(repo => repo.Delete(category));

            ValidationResponse<Category> actual = _categoryService.Delete(_categoryId);
            string actualName = actual.ResponseData.Name;

            Assert.Equal(category.Name, actualName);
            Assert.Equal(ValidationStatus.Ok, actual.Status);
        }

        [Fact(DisplayName = "Delete Category not found")]
        public void DeleteCategoryNotFoundTest()
        {
            string expectedMessage = $"Category with id {_categoryId} not found.";

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(() => null);

            ValidationResponse<Category> actual = _categoryService.Delete(_categoryId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.NotFound, actual.Status);
        }

        [Fact(DisplayName = "Delete Non-empty category")]
        public void DeleteNonEmptyCategoryTest()
        {
            Category category = GetCategory();
            List<Product> nonEmptyProductsList = new List<Product> { new Product() };

            string expectedMessage = $"Category with id {_categoryId} is not empty.";

            _mockRepository
                .Setup(repo => repo.GetById(_categoryId))
                .Returns(category);

            _mockProductRepository
                .Setup(repo => repo.GetByCategoryId(category.Id))
                .Returns(nonEmptyProductsList);

            ValidationResponse<Category> actual = _categoryService.Delete(_categoryId);

            Assert.Equal(expectedMessage, actual.Message);
            Assert.Equal(ValidationStatus.Failed, actual.Status);
        }

        private static Category GetCategory() =>
            new Category
            {
                Id = _categoryId,
                Name = "TestCategory"
            };
    }
}