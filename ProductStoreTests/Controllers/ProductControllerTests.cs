using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private ProductController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsOkWithProducts()
        {
            var mockProducts = new List<ProductResponse>();
            _mockProductService.Setup(service => service.GetProducts()).ReturnsAsync(mockProducts);

            var result = await _controller.GetProducts();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockProducts, okResult.Value);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsNotFound_WhenExceptionThrown()
        {
            _mockProductService.Setup(service => service.GetProducts()).ThrowsAsync(new Exception());

            var result = await _controller.GetProducts();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetProduct_ReturnsOkWithProduct_WhenProductExists()
        {
            int productId = 1;
            var mockProduct = new ProductResponse();
            _mockProductService.Setup(service => service.GetProductResponse(productId)).ReturnsAsync(mockProduct);

            var result = await _controller.GetProduct(productId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockProduct, okResult.Value);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            int productId = 1;
            _mockProductService.Setup(service => service.GetProductResponse(productId)).ThrowsAsync(new Exception());

            var result = await _controller.GetProduct(productId);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostProduct_ReturnsCreatedAtActionWithProduct_WhenProductIsCreated()
        {
            var productToAdd = new ProductRequest(); 
            var addedProduct = new ProductPostResponse(); 
            _mockProductService.Setup(service => service.PostProduct(productToAdd)).ReturnsAsync(addedProduct);

            var result = await _controller.PostProduct(productToAdd);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(addedProduct, createdAtActionResult.Value);
        }

        [TestMethod]
        public async Task PostProduct_ReturnsNotFound_WhenProductCreationFails()
        {
            var productToAdd = new ProductRequest(); // Populate with test data
            _mockProductService.Setup(service => service.PostProduct(productToAdd)).ThrowsAsync(new Exception());

            var result = await _controller.PostProduct(productToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsOkWithProduct_WhenProductIsDeleted()
        {
            int productId = 1;
            var deletedProduct = new ProductPostResponse(); // Populate with test data
            _mockProductService.Setup(service => service.DeleteProduct(productId)).ReturnsAsync(deletedProduct);

            var result = await _controller.DeleteProduct(productId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(deletedProduct, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            int productId = 1;
            _mockProductService.Setup(service => service.DeleteProduct(productId)).ThrowsAsync(new Exception());

            var result = await _controller.DeleteProduct(productId);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }

}
