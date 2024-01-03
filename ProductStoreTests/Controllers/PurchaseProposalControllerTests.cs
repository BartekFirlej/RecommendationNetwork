using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class PurchaseProposalControllerTests
    {
        private Mock<IPurchaseProposalService> _mockPurchaseProposalService;
        private PurchaseProposalController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseProposalService = new Mock<IPurchaseProposalService>();
            _controller = new PurchaseProposalController(_mockPurchaseProposalService.Object);
        }

        [TestMethod]
        public async Task GetPurchaseProposals_ReturnsOkWithPurchaseProposals()
        {
            var mockPurchaseProposals = new List<PurchaseProposalResponse>
            {
                new PurchaseProposalResponse { Id = 1, Date = DateTime.Now, CustomerId = 1, ProductId = 1 },
                new PurchaseProposalResponse { Id = 2, Date = DateTime.Now, CustomerId = 2, ProductId = 2 }
            };
            _mockPurchaseProposalService.Setup(service => service.GetPurchaseProposals()).ReturnsAsync(mockPurchaseProposals);

            var result = await _controller.GetPurchaseProposals();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockPurchaseProposals, okResult.Value);
        }

        [TestMethod]
        public async Task GetPurchaseProposals_ReturnsNotFound_WhenExceptionThrown()
        {
            _mockPurchaseProposalService.Setup(service => service.GetPurchaseProposals()).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetPurchaseProposals();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetPurchaseProposal_ReturnsOkWithPurchaseProposal()
        {
            int id = 1;
            var mockPurchaseProposal = new PurchaseProposalResponse { Id = id, Date = DateTime.Now, CustomerId = 1, ProductId = 1 };
            _mockPurchaseProposalService.Setup(service => service.GetPurchaseProposalResponse(id)).ReturnsAsync(mockPurchaseProposal);

            var result = await _controller.GetPurchaseProposal(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockPurchaseProposal, okResult.Value);
        }

        [TestMethod]
        public async Task GetPurchaseProposal_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockPurchaseProposalService.Setup(service => service.GetPurchaseProposalResponse(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetPurchaseProposal(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostPurchaseProposal_ReturnsCreatedAtActionWithPurchaseProposal()
        {
            var purchaseProposalToAdd = new PurchaseProposalRequest { Date = DateTime.Now, CustomerId = 1, ProductId = 1 };
            var addedPurchaseProposal = new PurchaseProposalResponse { Id = 3, Date = DateTime.Now, CustomerId = 1, ProductId = 1 };
            _mockPurchaseProposalService.Setup(service => service.PostPurchaseProposal(purchaseProposalToAdd)).ReturnsAsync(addedPurchaseProposal);

            var result = await _controller.PostPurchaseProposal(purchaseProposalToAdd);

            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            Assert.AreEqual(addedPurchaseProposal, createdAtResult.Value);
        }

        [TestMethod]
        public async Task PostPurchaseProposal_ReturnsNotFound_WhenExceptionThrown()
        {
            var purchaseProposalToAdd = new PurchaseProposalRequest { Date = DateTime.Now, CustomerId = 1, ProductId = 1 };
            _mockPurchaseProposalService.Setup(service => service.PostPurchaseProposal(purchaseProposalToAdd)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.PostPurchaseProposal(purchaseProposalToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeletePurchaseProposal_ReturnsOkWithPurchaseProposal()
        {
            int id = 1;
            var deletedPurchaseProposal = new PurchaseProposalResponse { Id = id, Date = DateTime.Now, CustomerId = 1, ProductId = 1 };
            _mockPurchaseProposalService.Setup(service => service.DeletePurchaseProposal(id)).ReturnsAsync(deletedPurchaseProposal);

            var result = await _controller.DeletePurchaseProposal(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(deletedPurchaseProposal, okResult.Value);
        }

        [TestMethod]
        public async Task DeletePurchaseProposal_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockPurchaseProposalService.Setup(service => service.DeletePurchaseProposal(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.DeletePurchaseProposal(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }

}
