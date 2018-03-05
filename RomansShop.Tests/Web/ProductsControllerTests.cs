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
using RomansShop.WebApi.ClientModels.Product;
using RomansShop.WebApi.Controllers;
using Xunit;

namespace RomansShop.Tests.Web
{
    public class ProductsControllerTests : UnitTestBase
    {
        private Mock<IProductService> _mockService;
        private Mock<IProductRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private ProductsController _controller;

        private static readonly Guid _categoryId = new Guid("00000000-0000-0000-0000-000000000002");
        private static readonly Guid _productId = new Guid("00000000-0000-0000-0000-000000000001");
        private static readonly string _productName = "TestProduct";

        public ProductsControllerTests()
        {
            _mockService = MockRepository.Create<IProductService>();
            _mockRepository = MockRepository.Create<IProductRepository>();
            _mockMapper = MockRepository.Create<IMapper>();

            _controller = new ProductsController(_mockService.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = "Get Product")]
        public void GetTest()
        {
            IEnumerable<Product> products = new List<Product> { GetProduct(), GetProduct() };

            IEnumerable<ProductResponseModel> productsResponse = 
                new List<ProductResponseModel> { GetProductResponseModel(), GetProductResponseModel() };

            _mockRepository
                .Setup(repo => repo.GetAll())
                .Returns(products);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products))
                .Returns(productsResponse);
            
            IActionResult actionResult = _controller.Get();

            OkObjectResult actual = (OkObjectResult)actionResult;
            int actualCount = ((IEnumerable<ProductResponseModel>)actual.Value).Count();

            Assert.Equal(products.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = "GetById Product")]
        public void GetByIdStatusTest()
        {
            ValidationResponse<Product> validationResponse = GetOkValidationResponse();
            ProductResponseModel productResponse = GetProductResponseModel();

            _mockService
                .Setup(serv => serv.GetById(_productId))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData))
                .Returns(productResponse);

            IActionResult actionResult = _controller.Get(_productId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Guid actualId = ((ProductResponseModel)actual.Value).Id;

            Assert.Equal(_productId, actualId);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = "GetById Product not found")]
        public void GetByIdProductNotFoundTest()
        {
            ValidationResponse<Product> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.GetById(_productId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Get(_productId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = "GetRange Products")]
        public void GetRangeTest()
        {
            IEnumerable<Product> expectedProducts = new List<Product> { GetProduct(), GetProduct() };

            ValidationResponse<IEnumerable<Product>> validationResponse = 
                new ValidationResponse<IEnumerable<Product>>(expectedProducts, ValidationStatus.Ok);

            IEnumerable<ProductResponseModel> productsResponse = 
                new List<ProductResponseModel> { GetProductResponseModel(), GetProductResponseModel() };

            int startIndex = 1;
            int offset = 2;

            _mockService
                .Setup(serv => serv.GetRange(startIndex, offset))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(validationResponse.ResponseData))
                .Returns(productsResponse);

            IActionResult actionResult = _controller.GetRange(startIndex, offset);

            ObjectResult actual = (ObjectResult)actionResult;
            int actualCount = ((IEnumerable<ProductResponseModel>)actual.Value).Count();

            Assert.Equal(expectedProducts.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = "GetRange Products Incorrect indexes")]
        public void GetRangeIncorrectIndexesTest()
        {
            int startIndex = -1;
            int offset = 0;
            string expectedMessage = $"The start index ({startIndex}) or offset ({offset}) is incorrect.";

            IActionResult actionResult = _controller.GetRange(startIndex, offset);

            BadRequestObjectResult actual = (BadRequestObjectResult)actionResult;

            Assert.Equal(expectedMessage, actual.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact(DisplayName = "Post Product")]
        public void PostTest()
        {
            Product product = GetProduct();
            ProductRequestModel productRequest = GetProductRequestModel();
            ProductResponseModel productResponse = GetProductResponseModel();

            _mockRepository
                .Setup(repo => repo.Add(product))
                .Returns(product);

            _mockMapper
                .Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest))
                .Returns(product);

            _mockMapper
                .Setup(mapper => mapper.Map<Product, ProductResponseModel>(product))
                .Returns(productResponse);

            IActionResult actionResult = _controller.Post(productRequest);

            CreatedAtActionResult actual = (CreatedAtActionResult)actionResult;
            string actualName = ((ProductResponseModel)actual.Value).Name;

            Assert.Equal(product.Name, actualName);
            Assert.Equal(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Fact(DisplayName = "Put Product")]
        public void PutTest()
        {
            Product product = GetProduct();
            ProductRequestModel productRequest = GetProductRequestModel();
            ProductResponseModel productResponse = GetProductResponseModel();

            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>(GetProduct(), ValidationStatus.Ok);

            _mockMapper
                .Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest))
                .Returns(product);

            _mockService
                .Setup(serv => serv.Update(product))
                .Returns(validationResponse);

            _mockMapper
                .Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData))
                .Returns(productResponse);

            IActionResult actionResult = _controller.Put(_productId, productRequest);

            OkObjectResult actual = (OkObjectResult)actionResult;
            string actualName = ((ProductResponseModel)actual.Value).Name;

            Assert.Equal(product.Name, actualName);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = "Put Product not found")]
        public void PutProductNotFoundTest()
        {
            Product product = GetProduct();
            ProductRequestModel productRequest = new ProductRequestModel();

            ValidationResponse<Product> validationResponse = GetNotFoundValidationResponse();

            _mockMapper
                .Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest))
                .Returns(product);

            _mockService
                .Setup(serv => serv.Update(product))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Put(_productId, productRequest);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = "Delete Product")]
        public void DeleteTest()
        {
            ValidationResponse<Product> validationResponse = GetOkValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_productId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_productId);

            OkObjectResult actual = (OkObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact(DisplayName = "Delete Product not found")]
        public void DeleteProductNotFoundTest()
        {
            ValidationResponse<Product> validationResponse = GetNotFoundValidationResponse();

            _mockService
                .Setup(serv => serv.Delete(_productId))
                .Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(_productId);

            NotFoundObjectResult actual = (NotFoundObjectResult)actionResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact(DisplayName = "GetByCategoryId Products")]
        public void GetByCategoryIdTest()
        {
            IEnumerable<Product> products = new List<Product> { GetProduct(), GetProduct() };

            IEnumerable<ProductResponseModel> productsResponse = 
                new List<ProductResponseModel> { GetProductResponseModel(), GetProductResponseModel() };

            _mockRepository
                .Setup(repo => repo.GetByCategoryId(_categoryId))
                .Returns(products);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products))
                .Returns(productsResponse);

            IActionResult actionResult = _controller.GetByCategoryId(_categoryId);

            IEnumerable<ProductResponseModel> expectedProductsResponse = new List<ProductResponseModel>();
            OkObjectResult actual = (OkObjectResult)actionResult;
            int actualCount = ((IEnumerable<ProductResponseModel>)actual.Value).Count();

            Assert.Equal(products.Count(), actualCount);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        private static Product GetProduct() => 
            new Product
            {
                Id = _productId,
                Name = _productName,
                CategoryId = _categoryId
            };

        private static ProductResponseModel GetProductResponseModel() =>
            new ProductResponseModel
            {
                Id = _productId,
                Name = _productName,
                CategoryId = _categoryId
            };

        private static ProductRequestModel GetProductRequestModel() =>
            new ProductRequestModel
            {
                Name = _productName
            };

        private ValidationResponse<Product> GetOkValidationResponse() => 
            new ValidationResponse<Product>(GetProduct(), ValidationStatus.Ok);

        private ValidationResponse<Product> GetNotFoundValidationResponse() =>
            new ValidationResponse<Product>(ValidationStatus.NotFound, "Not Found");

        private ValidationResponse<Product> GetFailedValidationResponse() =>
            new ValidationResponse<Product>(ValidationStatus.Failed, "Failed");
    }
}