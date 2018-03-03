using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RomansShop.Core;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
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

        #region GET Method Tests
        
        [Fact]
        public void Get_ReturnsStatusCodeOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponse> productsResponse = new List<ProductResponse>();

            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products)).Returns(productsResponse);

            // Act
            IActionResult actionResult = _controller.Get();

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Get_ReturnsProductResponseList()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponse> productsResponse = new List<ProductResponse>();

            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products)).Returns(productsResponse);

            // Act
            IActionResult actionResult = _controller.Get();

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actual.Value);
        }

        [Fact]
        public void GetById_ReturnsStatusCodeOk()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductResponse productResponse = new ProductResponse();

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(validationResponse.ResponseData)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Get(productId);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void GetById_ReturnsProductResponse()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductResponse productResponse = new ProductResponse();

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(validationResponse.ResponseData)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Get(productId);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        [Fact]
        public void GetById_ReturnsStatusNotFound()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockService.Setup(serv => serv.GetById(productId)).Returns(validationResponse);

            // Act
            IActionResult actionResult = _controller.Get(productId);

            // Assert
            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        [Fact]
        public void GetPage_ReturnsProductResponse_ForCorrectStartIndexAndOffset()
        {
            // Arrange
            ValidationResponse<IEnumerable<Product>> validationResponse = new ValidationResponse<IEnumerable<Product>>();
            IEnumerable<ProductResponse> productsResponse = new List<ProductResponse>();
            int startIndex = 1;
            int offset = 5;

            _mockService.Setup(serv => serv.GetPage(startIndex, offset)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(validationResponse.ResponseData)).Returns(productsResponse);

            // Act
            IActionResult actionResult = _controller.GetPage(startIndex, offset);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actual.Value);
        }

        [Fact]
        public void GetPage_ReturnsStatusBadRequest_ForNegativeOrZeroStartIndexOrOffset()
        {
            // Arrange
            ValidationResponse<IEnumerable<Product>> validationResponse 
                = new ValidationResponse<IEnumerable<Product>>() { Status = ValidationStatus.Failed };
            int startIndex = -1;
            int offset = 0;

            _mockService.Setup(serv => serv.GetPage(startIndex, offset)).Returns(validationResponse);

            // Act
            IActionResult actionResult = _controller.GetPage(startIndex, offset);

            // Assert
            BadRequestObjectResult actual = actionResult as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        #endregion

        #region POST Method Tests

        [Fact]
        public void Post_ReturnsStatusCodeCreated()
        {
            // Arrange
            Product product = new Product();
            ProductRequest createProductRequest = new ProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductRequest, Product>(createProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Post(createProductRequest);

            // Assert
            CreatedAtActionResult actual = actionResult as CreatedAtActionResult;
            Assert.Equal(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Fact]
        public void Post_ReturnsProductResponse()
        {
            // Arrange
            Product product = new Product();
            ProductRequest createProductRequest = new ProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductRequest, Product>(createProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Post(createProductRequest);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        #endregion

        #region PUT Method Tests

        [Fact]
        public void Put_ReturnsStatusCodeOk()
        {
            // Arrange
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductRequest productRequest = new ProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockMapper.Setup(mapper => mapper.Map<ProductRequest, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(validationResponse.ResponseData)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Put_ReturnsUpdatedProductResponse()
        {
            // Arrange
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();
            ProductRequest productRequest = new ProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockMapper.Setup(mapper => mapper.Map<ProductRequest, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(validationResponse.ResponseData)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        [Fact]
        public void Put_ReturnsStatusCodeNotFound_ForNonExistProduct()
        {
            // Arrange
            Guid updatedProductId = Guid.NewGuid();
            Product product = new Product() { Id = updatedProductId };

            ProductRequest productRequest = new ProductRequest();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockMapper.Setup(mapper => mapper.Map<ProductRequest, Product>(productRequest)).Returns(product);
            _mockService.Setup(serv => serv.Update(product)).Returns(validationResponse);

            // Act
            IActionResult actionResult = _controller.Put(updatedProductId, productRequest);

            // Assert
            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        #endregion

        #region DELETE Method Tests

        [Fact]
        public void Delete_ReturnsStatusCodeOk()
        {
            // Arrange
            Guid deletedProductId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = new ValidationResponse<Product>();

            _mockService.Setup(serv => serv.Delete(deletedProductId)).Returns(validationResponse);

            // Act
            IActionResult actionResult = _controller.Delete(deletedProductId);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsStatusCodeNotFound_WithNonExistId()
        {
            // Arrange
            Guid deletedProductId = Guid.NewGuid();
            ValidationResponse<Product> validationResponse = 
                new ValidationResponse<Product>() { Status = ValidationStatus.NotFound };

            _mockService.Setup(serv => serv.Delete(deletedProductId)).Returns(validationResponse);

            // Act
            IActionResult actionResult = _controller.Delete(deletedProductId);

            // Assert
            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        #endregion

        #region GetByCategoryId Tests

        [Fact]
        public void GetByCategoryId_ReturnsStatusCodeOk()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponse> productsResponse = new List<ProductResponse>();

            _mockRepository.Setup(repo => repo.GetByCategoryId(categoryId)).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products)).Returns(productsResponse);

            // Act
            IActionResult actionResult = _controller.GetByCategoryId(categoryId);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void GetByCategoryId_ReturnsProductResponseList()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<ProductResponse> productsResponse = new List<ProductResponse>();

            _mockRepository.Setup(repo => repo.GetByCategoryId(categoryId)).Returns(products);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products)).Returns(productsResponse);

            // Act
            IActionResult actionResult = _controller.GetByCategoryId(categoryId);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actual.Value);
        }

        #endregion
    }
}