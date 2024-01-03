using AutoMapper;
using Moq;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;

namespace ProductStoreTests.Services
{
    [TestClass]
    public class PurchaseProposalServiceTests
    {
        private Mock<IPurchaseProposalRepository> _mockPurchaseProposalRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IProductService> _mockProductService;
        private Mock<ICustomerService> _mockCustomerService;
        private PurchaseProposalService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseProposalRepository = new Mock<IPurchaseProposalRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockProductService = new Mock<IProductService>();
            _mockCustomerService = new Mock<ICustomerService>();
            _service = new PurchaseProposalService(_mockPurchaseProposalRepository.Object, _mockMapper.Object, _mockProductService.Object, _mockCustomerService.Object);
        }

        [TestMethod]
        public async Task GetPurchaseProposals_ReturnsProposalList_WhenFound()
        {
            var mockPurchaseProposals = new List<PurchaseProposalResponse>
            {
                new PurchaseProposalResponse { Id = 1, CustomerId = 1, ProductId = 1 },
                new PurchaseProposalResponse { Id = 2, CustomerId = 2, ProductId = 2 }
            };
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposals()).ReturnsAsync(mockPurchaseProposals);

            var result = await _service.GetPurchaseProposals();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.Id == 1));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any purchase proposals.")]
        public async Task GetPurchaseProposals_ThrowsException_WhenNotFound()
        {
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposals()).ReturnsAsync(new List<PurchaseProposalResponse>());

            await _service.GetPurchaseProposals();
        }

        [TestMethod]
        public async Task GetPurchaseProposal_ReturnsProposal_WhenFound()
        {
            int id = 1;
            var mockPurchaseProposal = new PurchaseProposal { Id = id };
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposal(id)).ReturnsAsync(mockPurchaseProposal);

            var result = await _service.GetPurchaseProposal(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetPurchaseProposal_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposal(id)).ReturnsAsync((PurchaseProposal)null);

            await _service.GetPurchaseProposal(id);
        }

        [TestMethod]
        public async Task GetPurchaseProposalResponse_ReturnsProposal_WhenFound()
        {
            int id = 1;
            var mockPurchaseProposal = new PurchaseProposalResponse { Id = id };
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposalResponse(id)).ReturnsAsync(mockPurchaseProposal);

            var result = await _service.GetPurchaseProposalResponse(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetPurchaseProposalResponse_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposalResponse(id)).ReturnsAsync((PurchaseProposalResponse)null);

            await _service.GetPurchaseProposalResponse(id);
        }

        [TestMethod]
        public async Task PostPurchaseProposal_ReturnsAddedProposal()
        {
            var purchaseProposalToAdd = new PurchaseProposalRequest { CustomerId = 1, ProductId = 1 };
            var addedPurchaseProposal = new PurchaseProposal { Id = 3 };
            var mappedResponse = new PurchaseProposalResponse { Id = 3 };

            _mockCustomerService.Setup(service => service.GetCustomer(purchaseProposalToAdd.CustomerId)).ReturnsAsync(new Customer());
            _mockProductService.Setup(service => service.GetProduct(purchaseProposalToAdd.ProductId)).ReturnsAsync(new Product());
            _mockPurchaseProposalRepository.Setup(repo => repo.PostPurchaseProposal(It.IsAny<PurchaseProposalRequest>())).ReturnsAsync(addedPurchaseProposal);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseProposalResponse>(addedPurchaseProposal)).Returns(mappedResponse);

            var result = await _service.PostPurchaseProposal(purchaseProposalToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Customer not found.")]
        public async Task PostPurchaseProposal_ThrowsException_WhenCustomerNotFound()
        {
            var purchaseProposalToAdd = new PurchaseProposalRequest { CustomerId = 99, ProductId = 1 };
            _mockCustomerService.Setup(service => service.GetCustomer(purchaseProposalToAdd.CustomerId)).ThrowsAsync(new Exception("Customer not found."));

            await _service.PostPurchaseProposal(purchaseProposalToAdd);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Product not found.")]
        public async Task PostPurchaseProposal_ThrowsException_WhenProductNotFound()
        {
            var purchaseProposalToAdd = new PurchaseProposalRequest { CustomerId = 1, ProductId = 99 };
            _mockProductService.Setup(service => service.GetProduct(purchaseProposalToAdd.ProductId)).ThrowsAsync(new Exception("Product not found."));

            await _service.PostPurchaseProposal(purchaseProposalToAdd);
        }


        [TestMethod]
        public async Task DeletePurchaseProposal_ReturnsDeletedProposal()
        {
            int id = 1;
            var purchaseProposalToDelete = new PurchaseProposal { Id = id };
            var mappedResponse = new PurchaseProposalResponse { Id = id };

            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposal(id)).ReturnsAsync(purchaseProposalToDelete);
            _mockPurchaseProposalRepository.Setup(repo => repo.DeletePurchaseProposal(purchaseProposalToDelete)).ReturnsAsync(purchaseProposalToDelete);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseProposalResponse>(purchaseProposalToDelete)).Returns(mappedResponse);

            var result = await _service.DeletePurchaseProposal(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Purchase proposal not found.")]
        public async Task DeletePurchaseProposal_ThrowsException_WhenProposalNotFound()
        {
            int id = 99;
            _mockPurchaseProposalRepository.Setup(repo => repo.GetPurchaseProposal(id)).ReturnsAsync((PurchaseProposal)null);

            await _service.DeletePurchaseProposal(id);
        }

    }

}
