using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RomansShop.Domain;
using RomansShop.Domain.Extensibility;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.Controllers;
using Xunit;

namespace RomansShop.Tests
{
    public class ProductsControllerTests
    {
        public ProductsControllerTests()
        {

        }

        #region GET Method Tests

        [Fact]
        public void Get_ReturnsAnObjectResult_WithAProductList()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProducts()).Returns(GetTestProducts());
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var products = controller.Get();

            // Assert
            var objResult = Assert.IsType<ObjectResult>(products);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(objResult.Value);
            Assert.Equal(GetTestProducts().Count(), model.Count());

        }

        [Fact]
        public void GetbyId_ReturnsAnObjectResult_WithAProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(GetTestProducts().ElementAt(0));
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Get(It.IsAny<int>());

            // Assert
            var objResult = Assert.IsType<ObjectResult>(product);
            var model = Assert.IsType<Product>(objResult.Value);
            Assert.Equal(GetTestProducts().ElementAt(0).Id, model.Id);

        }

        [Fact]
        public void GetbyId_ReturnsNotFound_WithNonExistId()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(null as Product);
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Get(It.IsAny<int>());

            // Assert
            var objResult = Assert.IsType<NotFoundResult>(product);

        }

        #endregion

        #region POST Method Tests

        [Fact]
        public void Post_ReturnsAnObjectResult_WithAddedProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.AddProduct(It.IsNotNull<Product>())).Returns(new Product() { Id = 1 });
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Post(new Product());

            // Assert
            var objResult = Assert.IsType<ObjectResult>(product);
            var model = Assert.IsType<Product>(objResult.Value);
            Assert.Equal(1, model.Id);

        }

        [Fact]
        public void Post_ReturnsBadRequestResult_ForNullProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.AddProduct(null as Product)).Returns(null as Product);
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Post(null as Product);

            // Assert
            var objResult = Assert.IsType<BadRequestResult>(product);

        }

        #endregion

        #region PUT Method Tests

        [Fact]
        public void Put_ReturnsOkResult_WithAttachedProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(new Product());
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Put(new Product());

            // Assert
            mockRepository.Verify(service => service.UpdateProduct(It.IsNotNull<Product>()), Times.Once());
            var objResult = Assert.IsType<OkResult>(product);
        }

        [Fact]
        public void Put_ReturnsBadRequestResult_WithNullProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Put(null);

            // Assert
            var objResult = Assert.IsType<BadRequestResult>(product);
        }

        [Fact]
        public void Put_ReturnsNotFoundResult_WithNonExistProduct()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(null as Product);
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Put(new Product());

            // Assert
            var objResult = Assert.IsType<NotFoundResult>(product);
        }

        #endregion

        #region DELETE Method Tests

        [Fact]
        public void Delete_ReturnsOkResult_WithId()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(new Product());
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Delete(It.IsAny<int>());

            // Assert
            mockRepository.Verify(service => service.DeleteProduct(It.IsAny<int>()), Times.Once());
            var objResult = Assert.IsType<OkResult>(product);
        }

        [Fact]
        public void Delete_ReturnsNotFoundResult_WithNonExistId()
        {

            // Arrange
            var mockService = new Mock<IProductService>();
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns(null as Product);
            var controller = new ProductsController(mockService.Object, mockRepository.Object);

            // Act
            var product = controller.Delete(It.IsAny<int>());

            // Assert
            var objResult = Assert.IsType<NotFoundResult>(product);
        }

        #endregion


        #region Utills

        private IEnumerable<Product> GetTestProducts()
        {
            var products = new List<Product>()
            {
                new Product() { Id = 0, Name = "Soap", Price = 10 },
                new Product() { Id = 1, Name = "Bread", Price = 5 }
            };

            return products;
        }

        #endregion
    }
}
