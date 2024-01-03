using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class ProductTypeControllerTests
    {
        private Mock<IProductTypeService> _mockProductTypeService;
        private ProductTypeController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockProductTypeService = new Mock<IProductTypeService>();
            _controller = new ProductTypeController(_mockProductTypeService.Object);
        }

        [TestMethod]
        public async Task GetProductTypes_ReturnsOkWithProductTypes()
        {
            var mockProductTypes = new List<ProductTypeResponse>
                {
                new ProductTypeResponse { Id = 1, Name = "Electronics" },
                new ProductTypeResponse { Id = 2, Name = "Books" }
                };
            _mockProductTypeService.Setup(service => service.GetProductTypes()).ReturnsAsync(mockProductTypes);

            var result = await _controller.GetProductTypes();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProductTypes = okResult.Value as List<ProductTypeResponse>;
            Assert.AreEqual(2, returnedProductTypes.Count);
            Assert.AreEqual("Electronics", returnedProductTypes[0].Name);
            Assert.AreEqual("Books", returnedProductTypes[1].Name);
        }

        [TestMethod]
        public async Task GetProductTypes_ReturnsNotFoundProductTypes()
        {
            _mockProductTypeService.Setup(service => service.GetProductTypes()).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetProductTypes();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetProductType_ReturnsOkWithProductType()
        {
            int id = 1;
            var mockProductType = new ProductTypeResponse { Id = id, Name = "Electronics" };
            _mockProductTypeService.Setup(service => service.GetProductTypeResponse(id)).ReturnsAsync(mockProductType);

            var result = await _controller.GetProductType(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProductType = okResult.Value as ProductTypeResponse;
            Assert.AreEqual(id, returnedProductType.Id);
            Assert.AreEqual("Electronics", returnedProductType.Name);
        }


        [TestMethod]
        public async Task GetProductType_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockProductTypeService.Setup(service => service.GetProductTypeResponse(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetProductType(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostProductType_ReturnsCreatedAtActionWithProductType()
        {
            var productTypeToAdd = new ProductTypeRequest { Name = "Furniture" };
            var addedProductType = new ProductTypeResponse { Id = 3, Name = "Furniture" };
            _mockProductTypeService.Setup(service => service.PostProductType(productTypeToAdd)).ReturnsAsync(addedProductType);

            var result = await _controller.PostProductType(productTypeToAdd);

            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            var returnedProductType = createdAtResult.Value as ProductTypeResponse;
            Assert.AreEqual(3, returnedProductType.Id);
            Assert.AreEqual("Furniture", returnedProductType.Name);
        }


        [TestMethod]
        public async Task PostProductType_ReturnsNotFound_WhenExceptionThrown()
        {
            var productTypeToAdd = new ProductTypeRequest { /* ... */ };
            _mockProductTypeService.Setup(service => service.PostProductType(productTypeToAdd)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.PostProductType(productTypeToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteProductType_ReturnsOkWithProductType()
        {
            int id = 2;
            var deletedProductType = new ProductTypeResponse { Id = id, Name = "Books" };
            _mockProductTypeService.Setup(service => service.DeleteProductType(id)).ReturnsAsync(deletedProductType);

            var result = await _controller.DeleteProductType(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProductType = okResult.Value as ProductTypeResponse;
            Assert.AreEqual(id, returnedProductType.Id);
            Assert.AreEqual("Books", returnedProductType.Name);
        }


        [TestMethod]
        public async Task DeleteProductType_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockProductTypeService.Setup(service => service.DeleteProductType(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.DeleteProductType(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }
}
