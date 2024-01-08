using Moq;
using ProductStore.Repositories;
using ProductStore.Services;
using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
namespace ProductStoreTests.Services
{
    [TestClass]
    public class PurchaseDetailServiceTests
    {
        private Mock<IPurchaseDetailRepository> _mockPurchaseDetailRepository;
        private Mock<IProductService> _mockProductService;
        private Mock<IMapper> _mockMapper;
        private PurchaseDetailService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseDetailRepository = new Mock<IPurchaseDetailRepository>();
            _mockProductService = new Mock<IProductService>();
            _mockMapper = new Mock<IMapper>();
            _service = new PurchaseDetailService(_mockPurchaseDetailRepository.Object, _mockProductService.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetPurchaseDetails_ReturnsAllDetails_WhenFound()
        {
            var mockDetails = new List<PurchaseDetailResponse> { new PurchaseDetailResponse()}; 
            _mockPurchaseDetailRepository.Setup(repo => repo.GetPurchaseDetails()).ReturnsAsync(mockDetails);

            var result = await _service.GetPurchaseDetails();

            Assert.AreEqual(mockDetails.Count, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetPurchaseDetails_ThrowsException_WhenNoDetailsFound()
        {
            _mockPurchaseDetailRepository.Setup(repo => repo.GetPurchaseDetails())
                 .ReturnsAsync(new List<PurchaseDetailResponse>());

            await _service.GetPurchaseDetails();
        }

        [TestMethod]
        public async Task AddPurchaseDetail_AddsDetailSuccessfully()
        {
            var purchaseDetailRequest = new PurchaseDetailRequest { ProductId = 1 };
            var purchaseDetail = new PurchaseDetailResponse();
            _mockProductService.Setup(service => service.GetProduct(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockPurchaseDetailRepository.Setup(repo => repo.AddPurchaseDetail(It.IsAny<PurchaseDetailRequest>(), It.IsAny<int>()))
                                         .ReturnsAsync(new PurchaseDetail());
            _mockMapper.Setup(mapper => mapper.Map<PurchaseDetailResponse>(It.IsAny<PurchaseDetail>()))
                       .Returns(purchaseDetail);

            var result = await _service.AddPurchaseDetail(purchaseDetailRequest, 1);

            Assert.AreEqual(purchaseDetail, result);
        }

    }
}

