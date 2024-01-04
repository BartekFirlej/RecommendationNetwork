using AutoMapper;
using Moq;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTests.Services
{
    [TestClass]
    public class PurchaseServiceTests
    {
        private Mock<IPurchaseRepository> _mockPurchaseRepository;
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IMapper> _mockMapper;
        private Mock<IPurchaseDetailService> _mockPurchaseDetailService;
        private PurchaseService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockCustomerService = new Mock<ICustomerService>();
            _mockMapper = new Mock<IMapper>();
            _mockPurchaseDetailService = new Mock<IPurchaseDetailService>();
            _service = new PurchaseService(_mockPurchaseRepository.Object, _mockCustomerService.Object, _mockMapper.Object, _mockPurchaseDetailService.Object);
        }

        [TestMethod]
        public async Task GetPurchases_ReturnsPurchases_WhenFound()
        {
            var mockPurchases = new List<PurchaseResponse> { 
                new PurchaseResponse {
                    Id = 1,
                    PurchaseDate = DateTime.Now,
                    CustomerId = 100,
                    RecommenderId = 200
                },
                new PurchaseResponse {
                    Id = 2,
                    PurchaseDate = DateTime.Now,
                    CustomerId = 100,
                    RecommenderId = 200
                } 
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchases()).ReturnsAsync(mockPurchases);

            var result = await _service.GetPurchases();

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any purchase.")]
        public async Task GetPurchases_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchases()).ReturnsAsync(new List<PurchaseResponse>());

            await _service.GetPurchases();
        }

        [TestMethod]
        public async Task GetPurchaseResponse_ReturnsPurchase_WhenFound()
        {
            var mockResponse = new PurchaseResponse
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseResponse(1)).ReturnsAsync(mockResponse);

            var result = await _service.GetPurchaseResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found purchase with id 99.")]
        public async Task GetPurchaseResponse_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseResponse(99)).ReturnsAsync((PurchaseResponse)null);

            await _service.GetPurchaseResponse(99);
        }

        [TestMethod]
        public async Task GetPurchase_ReturnsPurchase_WhenFound()
        {
            var mockPurchase = new Purchase
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchase(1)).ReturnsAsync(mockPurchase);

            var result = await _service.GetPurchase(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found purchase with id 99.")]
        public async Task GetPurchase_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchase(99)).ReturnsAsync((Purchase)null);

            await _service.GetPurchase(99);
        }


        [TestMethod]
        public async Task GetPurchaseWithDetails_ReturnsPurchase_WhenFound()
        {
            var mockResponse = new PurchaseWithDetailsResponse
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseWithDetails(1)).ReturnsAsync(mockResponse);

            var result = await _service.GetPurchaseWithDetails(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found purchase with id 99.")]
        public async Task GetPurchaseWithDetails_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseWithDetails(99)).ReturnsAsync((PurchaseWithDetailsResponse)null);

            await _service.GetPurchaseWithDetails(99);
        }

        [TestMethod]
        public async Task GetPurchasesWithDetails_ReturnsPurchases_WhenFound()
        {
            var mockPurchases = new List<PurchaseWithDetailsResponse>
            {
                new PurchaseWithDetailsResponse
                {
                    Id = 1,
                    PurchaseDate = new DateTime(2023, 1, 1),
                    CustomerId = 100,
                    RecommenderId = 200
                },
                new PurchaseWithDetailsResponse
                {
                    Id = 2,
                    PurchaseDate = new DateTime(2023, 1, 1),
                    CustomerId = 101,
                    RecommenderId = 201
                }
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchasesWithDetails()).ReturnsAsync(mockPurchases);

            var result = await _service.GetPurchasesWithDetails();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found purchase.")]
        public async Task GetPurchasesWithDetails_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchasesWithDetails()).ReturnsAsync(new List<PurchaseWithDetailsResponse>());

            await _service.GetPurchasesWithDetails();
        }

        [TestMethod]
        public async Task PostPurchase_CreatesPurchase_WhenValid()
        {
            var mockRequest = new PurchaseRequest
            {
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            var mockPurchase = new Purchase
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            var mockPurchaseResponse = new PurchaseResponse
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockCustomerService.Setup(service => service.GetCustomer(It.IsAny<int>())).ReturnsAsync(new Customer());
            _mockPurchaseRepository.Setup(repo => repo.PostPurchase(mockRequest)).ReturnsAsync(mockPurchase);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseResponse>(It.IsAny<Purchase>())).Returns(mockPurchaseResponse);

            var result = await _service.PostPurchase(mockRequest);

            Assert.AreEqual(mockPurchase.Id, result.Id);
        }


        [TestMethod]
        public async Task PostPurchaseWithDetails_CreatesPurchase_WhenValid()
        {
            var mockRequest = new PurchaseWithDetailsRequest
            {
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200,
                Products = new List<PurchaseDetailRequest>()
            };
            var mockPurchaseRequest = new PurchaseRequest
            {
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            var mockPurchase = new Purchase
            {
                Id = 3,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            var mockPurchaseResponse = new PurchaseWithDetailsResponse
            {
                Id = 3,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockCustomerService.Setup(service => service.GetCustomer(It.IsAny<int>())).ReturnsAsync(new Customer());
            _mockPurchaseRepository.Setup(repo => repo.PostPurchase(It.IsAny<PurchaseRequest>())).ReturnsAsync(mockPurchase);
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseWithDetails(It.IsAny<int>())).ReturnsAsync(mockPurchaseResponse);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseWithDetailsResponse>(It.IsAny<Purchase>())).Returns(mockPurchaseResponse);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseRequest>(mockRequest)).Returns(mockPurchaseRequest);

            var result = await _service.PostPurchaseWithDetails(mockRequest);

            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Wrong date")]
        public async Task PostPurchaseWithDetails_ThrowsException_WhenInvalidDate()
        {
            var mockRequest = new PurchaseWithDetailsRequest { PurchaseDate = DateTime.MinValue };

            await _service.PostPurchaseWithDetails(mockRequest);
        }

        [TestMethod]
        public async Task DeletePurchase_DeletesPurchase_WhenExists()
        {
            var mockPurchase = new Purchase
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            var mockPurchaseResponse = new PurchaseResponse
            {
                Id = 1,
                PurchaseDate = new DateTime(2023, 1, 1),
                CustomerId = 100,
                RecommenderId = 200
            };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchase(1)).ReturnsAsync(mockPurchase);
            _mockPurchaseRepository.Setup(repo => repo.DeletePurchase(mockPurchase)).ReturnsAsync(mockPurchase);
            _mockMapper.Setup(mapper => mapper.Map<PurchaseResponse>(It.IsAny<Purchase>())).Returns(mockPurchaseResponse);

            var result = await _service.DeletePurchase(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found purchase with id 99.")]
        public async Task DeletePurchase_ThrowsException_WhenNotFound()
        {
            _mockPurchaseRepository.Setup(repo => repo.GetPurchase(99)).ReturnsAsync((Purchase)null);

            await _service.DeletePurchase(99);
        }

    }
}
