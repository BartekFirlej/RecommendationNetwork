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
    public class PurchaseControllerTests
    {
        private Mock<IPurchaseService> _mockPurchaseService;
        private PurchaseController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseService = new Mock<IPurchaseService>();
            _controller = new PurchaseController(_mockPurchaseService.Object);
        }

        [TestMethod]
        public async Task GetPurchases_ReturnsOkWithPurchases()
        {
            var mockPurchases = new List<PurchaseResponse> { 
                new PurchaseResponse { Id = 1, CustomerId = 100, PurchaseDate = new DateTime(2023,1,1), RecommenderId = null },
                new PurchaseResponse { Id = 2, CustomerId = 101, PurchaseDate = new DateTime(2023,1,1), RecommenderId = null } 
            };
            _mockPurchaseService.Setup(service => service.GetPurchases()).ReturnsAsync(mockPurchases);

            var result = await _controller.GetPurchases();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockPurchases, okResult.Value);
        }

        [TestMethod]
        public async Task GetPurchases_ReturnsNotFoundOnException()
        {
            _mockPurchaseService.Setup(service => service.GetPurchases()).ThrowsAsync(new Exception());

            var result = await _controller.GetPurchases();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetPurchase_ReturnsOkWithPurchase()
        {
            var mockPurchase = new PurchaseResponse { Id = 1, CustomerId = 100, PurchaseDate = new DateTime(2023, 1, 1), RecommenderId = null };
            _mockPurchaseService.Setup(service => service.GetPurchaseResponse(1)).ReturnsAsync(mockPurchase);

            var result = await _controller.GetPurchase(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetPurchase_ReturnsNotFound()
        {
            _mockPurchaseService.Setup(service => service.GetPurchaseResponse(99)).ThrowsAsync(new Exception());

            var result = await _controller.GetPurchase(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostPurchase_ReturnsCreatedAtWithPurchase()
        {
            var mockRequest = new PurchaseRequest { CustomerId = 100, PurchaseDate = new DateTime(2023, 1, 1), RecommenderId = null };
            var mockResponse = new PurchaseResponse { Id = 1, CustomerId = 100, PurchaseDate = new DateTime(2023, 1, 1), RecommenderId = null };
            _mockPurchaseService.Setup(service => service.PostPurchase(mockRequest)).ReturnsAsync(mockResponse);

            var result = await _controller.PostPurchase(mockRequest);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task PostPurchase_ReturnsNotFoundOnException()
        {
            var mockRequest = new PurchaseRequest { CustomerId = 100, PurchaseDate = new DateTime(2023, 1, 1), RecommenderId = null };
            _mockPurchaseService.Setup(service => service.PostPurchase(mockRequest)).ThrowsAsync(new Exception());

            var result = await _controller.PostPurchase(mockRequest);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeletePurchase_ReturnsOk()
        {
            var mockResponse = new PurchaseResponse { Id = 1, CustomerId = 100, PurchaseDate = new DateTime(2023, 1, 1), RecommenderId = null };
            _mockPurchaseService.Setup(service => service.DeletePurchase(1)).ReturnsAsync(mockResponse);

            var result = await _controller.DeletePurchase(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeletePurchase_ReturnsNotFoundOnException()
        {
            _mockPurchaseService.Setup(service => service.DeletePurchase(99)).ThrowsAsync(new Exception());

            var result = await _controller.DeletePurchase(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
