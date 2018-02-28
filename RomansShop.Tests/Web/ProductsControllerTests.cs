using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            Product product = new Product();
            ProductResponse productResponse = new ProductResponse();
            _mockRepository.Setup(repo => repo.GetById(product.Id)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Get(product.Id);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void GetById_ReturnsProductResponse()
        {
            // Arrange
            Product product = new Product();
            ProductResponse productResponse = new ProductResponse();
            _mockRepository.Setup(repo => repo.GetById(product.Id)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Get(product.Id);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        [Fact]
        public void GetById_ReturnsStatusNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(() => null);

            // Act
            IActionResult actionResult = _controller.Get(It.IsAny<Guid>());

            // Assert
            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        #endregion

        #region POST Method Tests

        [Fact]
        public void Post_ReturnsStatusCodeCreated()
        {
            // Arrange
            Product product = new Product();
            CreateProductRequest createProductRequest = new CreateProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<CreateProductRequest, Product>(createProductRequest)).Returns(product);
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
            CreateProductRequest createProductRequest = new CreateProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.Add(product)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<CreateProductRequest, Product>(createProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Post(createProductRequest);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        [Fact]
        public void Post_ReturnsStatusCodeBadRequest_ForNullProduct()
        {
            // Arrange

            // Act
            IActionResult actionResult = _controller.Post(null);

            // Assert
            BadRequestObjectResult actual = actionResult as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        #endregion

        #region PUT Method Tests

        [Fact]
        public void Put_ReturnsStatusCodeOk()
        {
            // Arrange
            Product product = new Product();
            EditProductRequest editProductRequest = new EditProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.GetById(product.Id)).Returns(product);
            _mockRepository.Setup(repo => repo.Update(product)).Returns(product);

            _mockMapper.Setup(mapper => mapper.Map<EditProductRequest, Product>(editProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Put(editProductRequest);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Put_ReturnsUpdatedProductResponse()
        {
            // Arrange
            Product product = new Product();
            EditProductRequest editProductRequest = new EditProductRequest();
            ProductResponse productResponse = new ProductResponse();

            _mockRepository.Setup(repo => repo.GetById(product.Id)).Returns(product);
            _mockRepository.Setup(repo => repo.Update(product)).Returns(product);

            _mockMapper.Setup(mapper => mapper.Map<EditProductRequest, Product>(editProductRequest)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<Product, ProductResponse>(product)).Returns(productResponse);

            // Act
            IActionResult actionResult = _controller.Put(editProductRequest);

            // Assert
            ObjectResult actual = actionResult as ObjectResult;
            Assert.IsAssignableFrom<ProductResponse>(actual.Value);
        }

        [Fact]
        public void Put_ReturnsStatusCodeBadRequest_ForNullProduct()
        {
            // Arrange

            // Act
            IActionResult actionResult = _controller.Put(null);

            // Assert
            BadRequestObjectResult actual = actionResult as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact]
        public void Put_ReturnsStatusCodeNotFound_ForNonExistProduct()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(() => null);

            // Act
            IActionResult actionResult = _controller.Put(new EditProductRequest());

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
            Product deletedProduct = new Product();
            _mockRepository.Setup(repo => repo.GetById(deletedProduct.Id)).Returns(deletedProduct);
            _mockRepository.Setup(repo => repo.Delete(deletedProduct));

            // Act
            IActionResult actionResult = _controller.Delete(deletedProduct.Id);

            // Assert
            OkObjectResult actual = actionResult as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsStatusCodeNotFound_WithNonExistId()
        {
            // Arrange
            EditProductRequest deletedProduct = new EditProductRequest();
            _mockRepository.Setup(repo => repo.GetById(deletedProduct.Id)).Returns(() => null);

            // Act
            IActionResult actionResult = _controller.Delete(deletedProduct.Id);

            // Assert
            NotFoundObjectResult actual = actionResult as NotFoundObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
        }

        #endregion
    }
}
