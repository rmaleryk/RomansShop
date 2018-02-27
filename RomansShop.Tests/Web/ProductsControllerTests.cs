using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.Controllers;
using Xunit;

namespace RomansShop.Tests
{
    public class ProductsControllerTests
    {
        public ProductsControllerTests()
        { }

        #region GET Method Tests

        [Fact]
        public void Get_ReturnsAnObjectResult_WithAProductList()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(GetTestProducts());
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult productsRes = controller.Get();

            // Assert
            OkObjectResult objResult = Assert.IsType<OkObjectResult>(productsRes);
            IEnumerable<Product> products = Assert.IsAssignableFrom<IEnumerable<Product>>(objResult.Value);
            Assert.Equal(GetTestProducts().Count(), products.Count());
        }
        
        [Fact]
        public void GetById_ReturnsAnObjectResult_WithAProduct()
        {
            Product expectedProduct = GetTestProducts().ElementAt(0);

            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(expectedProduct);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult productRes = controller.Get(It.IsAny<Guid>());

            // Assert
            OkObjectResult objResult = Assert.IsType<OkObjectResult>(productRes);
            Product actualProduct = Assert.IsType<Product>(objResult.Value);
            Assert.Equal(expectedProduct.Id, actualProduct.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WithNonExistId()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(() => null);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Get(It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(product);
        }

        #endregion

        #region POST Method Tests

        [Fact]
        public void Post_ReturnsAnObjectResult_WithAddedProduct()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.Add(It.IsNotNull<Product>())).Returns(new Product() { Id = new Guid() });
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult productRes = controller.Post(new Product());

            // Assert
            CreatedAtActionResult objResult = Assert.IsType<CreatedAtActionResult>(productRes);
            Product product = Assert.IsType<Product>(objResult.Value);
            Assert.Equal(new Guid(), product.Id); // TODO: GUID creation once
        }

        [Fact]
        public void Post_ReturnsBadRequestResult_ForNullProduct()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.Add(null as Product)).Returns(() => null);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Post(null as Product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(product);
        }

        #endregion

        #region PUT Method Tests

        [Fact]
        public void Put_ReturnsOkResult_WithAttachedProduct()
        {
            Product expectedProduct = GetTestProducts().ElementAt(0);

            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(expectedProduct);
            mockRepository.Setup(repo => repo.Update(expectedProduct)).Returns(expectedProduct);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Put(expectedProduct);

            // Assert
            mockRepository.Verify(repo => repo.Update(It.IsNotNull<Product>()), Times.Once());
            OkObjectResult objResult = Assert.IsType<OkObjectResult>(product);
            Product actualProduct = Assert.IsType<Product>(objResult.Value);
            Assert.Equal(expectedProduct.Id, actualProduct.Id);
        }

        [Fact]
        public void Put_ReturnsBadRequestResult_WithNullProduct()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Put(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(product);
        }

        [Fact]
        public void Put_ReturnsNotFoundResult_WithNonExistProduct()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(() => null);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Put(new Product());

            // Assert
            Assert.IsType<NotFoundObjectResult>(product);
        }

        #endregion

        #region DELETE Method Tests

        [Fact]
        public void Delete_ReturnsOkResult_WithId()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(new Guid())).Returns(new Product());
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Delete(new Product());

            // Assert
            mockRepository.Verify(service => service.Delete(It.IsAny<Product>()), Times.Once());
            Assert.IsType<OkResult>(product);
        }

        [Fact]
        public void Delete_ReturnsNotFoundResult_WithNonExistId()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(() => null);
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Delete(GetTestProducts().ElementAt(0));

            // Assert
            Assert.IsType<NotFoundObjectResult>(product);
        }

        [Fact]
        public void Delete_ReturnsBadRequestResult_WithNullProduct()
        {
            // Arrange
            Mock<IProductService> mockService = new Mock<IProductService>();
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            ProductsController controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            IActionResult product = controller.Delete(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(product);
        }

        #endregion


        #region Utills

        private IEnumerable<Product> GetTestProducts()
        {
            var products = new List<Product>()
            {
                new Product() { Id = new Guid(), Name = "Soap", Price = 10 },
                new Product() { Id = new Guid(), Name = "Bread", Price = 5 }
            };

            return products;
        }

        #endregion
    }
}
