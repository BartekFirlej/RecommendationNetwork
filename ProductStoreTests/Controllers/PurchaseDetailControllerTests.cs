using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class PurchaseDetailControllerTests
    {
        private Mock<IPurchaseDetailService> _mockService;
        private PurchaseDetailController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<IPurchaseDetailService>();
            _controller = new PurchaseDetailController(_mockService.Object);
        }

        [TestMethod]
        public async Task GetPurchaseDetails_ReturnsOkObjectResult_WithPurchaseDetails()
        {
            var mockResponse = new List<PurchaseDetailResponse>();
            _mockService.Setup(service => service.GetPurchaseDetails()).ReturnsAsync(mockResponse);

            var result = await _controller.GetPurchaseDetailss();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockResponse, okResult.Value);
        }

        [TestMethod]
        public async Task GetPurchaseDetails_ReturnsNotFoundResult_WhenExceptionThrown()
        {
            _mockService.Setup(service => service.GetPurchaseDetails()).ThrowsAsync(new Exception());

            var result = await _controller.GetPurchaseDetailss();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostPurchaseDetail_ReturnsCreatedAtActionResult_WithPurchaseDetail()
        {
            var purchaseToAdd = new PurchaseIdDetailRequest();
            var mockResponse = new PurchaseDetailResponse();
            _mockService.Setup(service => service.AddPurchaseDetail(purchaseToAdd)).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.PostPurchaseDetail(purchaseToAdd);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(mockResponse, createdResult.Value);
        }

        [TestMethod]
        public async Task DeletePurchaseDetail_ReturnsOkObjectResult_WithDeletedDetail()
        {
            int id = 1;
            var mockResponse = new PurchaseDetailResponse();
            _mockService.Setup(service => service.DeletePurchaseDetail(id)).ReturnsAsync(mockResponse);

            var result = await _controller.DeletePurchaseDetail(id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockResponse, okResult.Value);
        }

        [TestMethod]
        public async Task DeletePurchaseDetail_ReturnsNotFoundResult_WhenExceptionThrown()
        {
            int id = 1;
            _mockService.Setup(service => service.DeletePurchaseDetail(id)).ThrowsAsync(new Exception());

            var result = await _controller.DeletePurchaseDetail(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // Test for successful response
        [TestMethod]
        public async Task GetPurchaseDetail_ReturnsOkObjectResult_WithPurchaseDetail()
        {
            var mockResponse = new PurchaseDetailResponse();
            _mockService.Setup(s => s.GetPurchaseDetailResponse(It.IsAny<int>())).ReturnsAsync(mockResponse);

            var result = await _controller.GetPurchaseDetail(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockResponse, okResult.Value);
        }

        [TestMethod]
        public async Task GetPurchaseDetail_ReturnsNotFoundResult_WhenExceptionThrown()
        {
            _mockService.Setup(s => s.GetPurchaseDetailResponse(It.IsAny<int>())).ThrowsAsync(new Exception());

            var result = await _controller.GetPurchaseDetail(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostPurchaseDetail_ReturnsNotFoundResult_WhenExceptionThrown()
        {
            var purchaseToAdd = new PurchaseIdDetailRequest();
            _mockService.Setup(s => s.AddPurchaseDetail(It.IsAny<PurchaseIdDetailRequest>())).ThrowsAsync(new Exception());

            var result = await _controller.PostPurchaseDetail(purchaseToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
