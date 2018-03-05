using System;
using System.Collections.Generic;
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
        private Mock<IProductService> _mockService { get; set; }
        private Mock<IProductRepository> _mockRepository { get; set; }
        private Mock<IMapper> _mockMapper { get; set; }
        private ProductsController _controller { get; set; }

        public ProductsControllerTests()
        {
            _mockService = MockRepository.Create<IProductService>();
            _mockRepository = MockRepository.Create<IProductRepository>();
            _mockMapper = MockRepository.Create<IMapper>();

            _controller = new ProductsController(_mockService.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void Get_ReturnsStatusCodeOk()
        {
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponseModel> productsResponse = new List<ProductResponseModel>();

            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products)).Returns(productsResponse);

            IActionResult actionResult = _controller.Get();

            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Get_ReturnsProductResponseList()
        {
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponseModel> productsResponse = new List<ProductResponseModel>();

            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products)).Returns(productsResponse);

            IActionResult actionResult = _controller.Get();

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponseModel>>(actual.Value);
        }

        [Fact]
        public void GetById_ReturnsStatusCodeOk()
        {
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData)).Returns(productResponse);

            IActionResult actionResult = _controller.Get(productId);

            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void GetById_ReturnsProductResponse()
        {
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData)).Returns(productResponse);

            IActionResult actionResult = _controller.Get(productId);

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponseModel>(actual.Value);
        }

        [Fact]
        public void GetById_ReturnsStatusNotFound()
        {
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);

            IActionResult actionResult = _controller.Get(productId);

            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        public void GetPage_ReturnsProductResponse_ForCorrectStartIndexAndOffset()
        {
            ValidationResponse<IEnumerable<Product>> validationResponse = new ValidationResponse<IEnumerable<Product>>();
            IEnumerable<ProductResponseModel> productsResponse = new List<ProductResponseModel>();
            int startIndex = 1;
            int offset = 5;

            _mockService.Setup(serv => serv.GetRange(startIndex, offset)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(validationResponse.ResponseData)).Returns(productsResponse);

            IActionResult actionResult = _controller.GetRange(startIndex, offset);

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponseModel>>(actual.Value);
        }

        [Fact]
        public void GetPage_ReturnsStatusBadRequest_ForNegativeOrZeroStartIndexOrOffset()
        {
            ValidationResponse<IEnumerable<Product>> validationResponse 
                = new ValidationResponse<IEnumerable<Product>>() { Status = ValidationStatus.Failed };
            int startIndex = -1;
            int offset = 0;

            _mockService.Setup(serv => serv.GetRange(startIndex, offset)).Returns(validationResponse);

            IActionResult actionResult = _controller.GetRange(startIndex, offset);

            BadRequestObjectResult actual = actionResult as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact]
        public void Post_ReturnsStatusCodeCreated()
        {
            Product product = new Product();
            ProductRequestModel createProductRequest = new ProductRequestModel();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductRequestModel, Product>(createProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(product)).Returns(productResponse);

            IActionResult actionResult = _controller.Post(createProductRequest);

            CreatedAtActionResult actual = actionResult as CreatedAtActionResult;
            Assert.Equal(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Fact]
        public void Post_ReturnsProductResponse()
        {
            Product product = new Product();
            ProductRequestModel createProductRequest = new ProductRequestModel();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductRequestModel, Product>(createProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(product)).Returns(productResponse);

            IActionResult actionResult = _controller.Post(createProductRequest);

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponseModel>(actual.Value);
        }

        [Fact]
        public void Put_ReturnsStatusCodeOk()
        {
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductRequestModel productRequest = new ProductRequestModel();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockMapper.Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData)).Returns(productResponse);

            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Put_ReturnsUpdatedProductResponse()
        {
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductRequestModel productRequest = new ProductRequestModel();
            ProductResponseModel productResponse = new ProductResponseModel();

            _mockMapper.Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponseModel>(validationResponse.ResponseData)).Returns(productResponse);

            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponseModel>(actual.Value);
        }

        [Fact]
        public void Put_ReturnsStatusCodeNotFound_ForNonExistProduct()
        {
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ProductRequestModel productRequest = new ProductRequestModel();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockMapper.Setup(mapper => mapper.Map<ProductRequestModel, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);

            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsStatusCodeOk()
        {
            Guid deletedProductId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();

            _mockService.Setup(serv => serv.Delete(deletedProductId)).Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(deletedProductId);

            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsStatusCodeNotFound_WithNonExistId()
        {
            Guid deletedProductId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockService.Setup(serv => serv.Delete(deletedProductId)).Returns(validationResponse);

            IActionResult actionResult = _controller.Delete(deletedProductId);

            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        public void GetByCategoryId_ReturnsStatusCodeOk()
        {
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponseModel> productsResponse = new List<ProductResponseModel>();

            _mockRepository.Setup(repo => repo.GetByCategoryId(categoryId)).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products)).Returns(productsResponse);

            IActionResult actionResult = _controller.GetByCategoryId(categoryId);

            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void GetByCategoryId_ReturnsProductResponseList()
        {
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponseModel> productsResponse = new List<ProductResponseModel>();

            _mockRepository.Setup(repo => repo.GetByCategoryId(categoryId)).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponseModel>>(products)).Returns(productsResponse);

            IActionResult actionResult = _controller.GetByCategoryId(categoryId);

            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponseModel>>(actual.Value);
        }
    }
}