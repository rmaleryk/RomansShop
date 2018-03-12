using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.Tests.Common;
using RomansShop.WebApi.ClientModels.Category;
using RomansShop.WebApi.Controllers;
using Xunit;

namespace RomansShop.Tests.Web
{
    public class CategoriesControllerTest : UnitTestBase
    {
        private Mock<ICategoryService> _mockService;
        private Mock<ICategoryRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private CategoriesController _controller;

        private static readonly string _categoryName = "TestCategory";
        private static readonly Guid _categoryId = new Guid("00000000-0000-0000-0000-000000000002");

        const string GetMethodName = nameof(CategoriesController.Get) + ". ";
        const string GetByIdMethodName = nameof(CategoriesController.GetById) + ". ";
        const string PostMethodName = nameof(CategoriesController.Post) + ". ";
        const string PutMethodName = nameof(CategoriesController.Put) + ". ";
        const string DeleteMethodName = nameof(CategoriesController.Delete) + ". ";

        public CategoriesControllerTest()
        {
            _mockService = MockRepository.Create<ICategoryService>();
            _mockRepository = MockRepository.Create<ICategoryRepository>();
            _mockMapper = MockRepository.Create<IMapper>();

            _controller = new CategoriesController(_mockService.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = GetMethodName)]
        public void GetTest()
        {
            IEnumerable<Category> categories = new List<Category> { GetCategory(), GetCategory() };

            IEnumerable<CategoryResponseModel> categoriesResponse = 
                new List<CategoryResponseModel> { GetCategoryResponseModel(), GetCategoryResponseModel() };

            _mockRepository
                .Setup(repo => repo.GetAll())
                .Returns(categories);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponseModel>>(categories))
                .Returns(categoriesResponse);
            
            IActionResult actionResult = _controller.Get();

            OkObjectResult actual = (OkObjectResult)actionResult;
            int actualCount = ((IEnumerable<CategoryResponseModel>)actual.Value).Count();

            Assert.Equal(categories.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = GetByIdMethodName)]
        public void GetByIdTest()
        {
            ValidationResponse<Category> validationResponse = GetOkValidationResponse();
            CategoryResponseModel categoryResponse = GetCategoryResponseModel();

            _mockService
                .Setup(serv => serv.GetById(_categoryId))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData))
                .Returns(categoryResponse);

            IActionResult actionResult = _controller.GetById(_categoryId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Guid actualId = ((CategoryResponseModel)actual.Value).Id;

            Assert.Equal(_categoryId, actualId);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = GetByIdMethodName + "Category not found")]
        public void GetByIdCategoryNotFoundTest()
        {
            ValidationResponse<Category> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.GetById(_categoryId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.GetById(_categoryId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = PostMethodName)]
        public void PostTest()
        {
            Category category = GetCategory();
            ValidationResponse<Category> validationResponse = GetOkValidationResponse();
            CategoryRequestModel categoryRequest = GetCategoryRequestModel();
            CategoryResponseModel categoryResponse = GetCategoryResponseModel();

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryRequestModel, Category>(categoryRequest))
                .Returns(category);

            _mockService
                .Setup(serv => serv.Add(category))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData))
                .Returns(categoryResponse);

            IActionResult actionResult = _controller.Post(categoryRequest);

            CreatedAtActionResult actual = (CreatedAtActionResult)actionResult;
            string actualName = ((CategoryResponseModel)actual.Value).Name;

            Assert.Equal(category.Name, actualName);
            Assert.Equal(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Fact(DisplayName = PostMethodName + "Category already exist")]
        public void PostCategoryAlreadyExistTest()
        {
            Category category = GetCategory();
            ValidationResponse<Category> validationResponse = GetFailedValidationResponse();
            CategoryRequestModel categoryRequest = GetCategoryRequestModel();

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryRequestModel, Category>(categoryRequest))
                .Returns(category);

            _mockService
                .Setup(serv => serv.Add(category))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Post(categoryRequest);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName)]
        public void PutTest()
        {
            Category category = GetCategory();
            CategoryRequestModel categoryRequest = GetCategoryRequestModel();
            CategoryResponseModel categoryResponse = GetCategoryResponseModel();
            ValidationResponse<Category> validationResponse = GetOkValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryRequestModel, Category>(categoryRequest))
                .Returns(category);

            _mockService
                .Setup(serv => serv.Update(category))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<Category, CategoryResponseModel>(validationResponse.ResponseData))
                .Returns(categoryResponse);

            IActionResult actionResult = _controller.Put(_categoryId, categoryRequest);

            OkObjectResult actual = (OkObjectResult)actionResult;
            string actualName = ((CategoryResponseModel)actual.Value).Name;

            Assert.Equal(category.Name, actualName);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName + "Category not found")]
        public void PutCategoryNotFoundTest()
        {
            Category category = GetCategory();
            CategoryRequestModel categoryRequest = GetCategoryRequestModel();
            ValidationResponse<Category> validationResponse = GetNotFoundValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryRequestModel, Category>(categoryRequest))
                .Returns(category);

            _mockService
                .Setup(serv => serv.Update(category))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Put(_categoryId, categoryRequest);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = PutMethodName + "Category already exist")]
        public void PutCategoryAlreadyExistTest()
        {
            Category category = GetCategory();
            CategoryRequestModel categoryRequest = GetCategoryRequestModel();
            ValidationResponse<Category> validationResponse = GetFailedValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryRequestModel, Category>(categoryRequest))
                .Returns(category);

            _mockService
                .Setup(serv => serv.Update(category))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Put(_categoryId, categoryRequest);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = DeleteMethodName)]
        public void DeleteTest()
        {
            ValidationResponse<Category> validationResponse = GetOkValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_categoryId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_categoryId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = DeleteMethodName + "Category not found")]
        public void DeleteCategoryNotFoundTest()
        {
            ValidationResponse<Category> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_categoryId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_categoryId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = DeleteMethodName + "Non empty category")]
        public void DeleteNonEmptyCategoryTest()
        {
            ValidationResponse<Category> validationResponse = GetFailedValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_categoryId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_categoryId);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        private static Category GetCategory() => 
            new Category
            {
                Id = _categoryId,
                Name = _categoryName
            };

        private static CategoryResponseModel GetCategoryResponseModel() =>
            new CategoryResponseModel
            {
                Id = _categoryId,
                Name = _categoryName
            };

        private static CategoryRequestModel GetCategoryRequestModel() =>
            new CategoryRequestModel
            {
                Name = _categoryName
            };

        private ValidationResponse<Category> GetOkValidationResponse() => 
            new ValidationResponse<Category>(GetCategory(), ValidationStatus.Ok);

        private ValidationResponse<Category> GetNotFoundValidationResponse() =>
            new ValidationResponse<Category>(ValidationStatus.NotFound, "Not Found");

        private ValidationResponse<Category> GetFailedValidationResponse() =>
            new ValidationResponse<Category>(ValidationStatus.Failed, "Failed");
    }
}