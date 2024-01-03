using AutoMapper;
using Moq;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;

namespace ProductStoreTests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IProductTypeService> _mockProductTypeService;
        private Mock<IMapper> _mockMapper;
        private ProductService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductTypeService = new Mock<IProductTypeService>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProductService(_mockProductRepository.Object, _mockProductTypeService.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsProductList_WhenFound()
        {
            var mockProducts = new List<ProductResponse>
                {
                new ProductResponse { Id = 1, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1, ProductTypeName = "Electronics" },
                new ProductResponse { Id = 2, ProductName = "Book", Price = 30.00f, ProductTypeId = 2, ProductTypeName = "Books" }
                };
            _mockProductRepository.Setup(repo => repo.GetProducts()).ReturnsAsync(mockProducts);

            var result = await _service.GetProducts();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Laptop", result.First().ProductName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any products.")]
        public async Task GetProducts_ThrowsException_WhenNotFound()
        {
            _mockProductRepository.Setup(repo => repo.GetProducts()).ReturnsAsync(new List<ProductResponse>());

            await _service.GetProducts();
        }

        [TestMethod]
        public async Task GetProductResponse_ReturnsProduct_WhenFound()
        {
            int id = 1;
            var mockProduct = new ProductResponse { Id = id, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1, ProductTypeName = "Electronics" };
            _mockProductRepository.Setup(repo => repo.GetProductResponse(id)).ReturnsAsync(mockProduct);

            var result = await _service.GetProductResponse(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Laptop", result.ProductName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetProductResponse_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockProductRepository.Setup(repo => repo.GetProductResponse(id)).ReturnsAsync((ProductResponse)null);

            await _service.GetProductResponse(id);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsProduct_WhenFound()
        {
            int id = 1;
            var mockProduct = new Product { Id = id, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 };
            _mockProductRepository.Setup(repo => repo.GetProduct(id)).ReturnsAsync(mockProduct);

            var result = await _service.GetProduct(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Laptop", result.ProductName);
            Assert.AreEqual(1200.00f, result.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetProduct_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockProductRepository.Setup(repo => repo.GetProduct(id)).ReturnsAsync((Product)null);

            await _service.GetProduct(id);
        }

        [TestMethod]
        public async Task PostProduct_ReturnsAddedProduct_WhenSuccessful()
        {
            var productToAdd = new ProductRequest { ProductName = "Chair", Price = 100.00f, ProductTypeId = 3 };
            var addedProduct = new Product { Id = 3, ProductName = "Chair", Price = 100.00f, ProductTypeId = 3 };
            var mappedResponse = new ProductPostResponse { Id = 3, ProductName = "Chair", Price = 100.00f };

            _mockProductTypeService.Setup(service => service.GetProductType(productToAdd.ProductTypeId)).ReturnsAsync(new ProductType { Id = 3, Name = "Furniture" });
            _mockProductRepository.Setup(repo => repo.PostProduct(productToAdd)).ReturnsAsync(addedProduct);
            _mockMapper.Setup(mapper => mapper.Map<ProductPostResponse>(addedProduct)).Returns(mappedResponse);

            var result = await _service.PostProduct(productToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual("Chair", result.ProductName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Product type not found.")]
        public async Task PostProduct_ThrowsException_WhenProductTypeNotFound()
        {
            var productToAdd = new ProductRequest { ProductName = "InvalidProduct", Price = 100.00f, ProductTypeId = 99 };
            _mockProductTypeService.Setup(service => service.GetProductType(productToAdd.ProductTypeId)).ThrowsAsync(new Exception("Product type not found."));

            await _service.PostProduct(productToAdd);
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsDeletedProduct_WhenSuccessful()
        {
            int id = 1;
            var productToDelete = new Product { Id = id, ProductName = "Laptop", Price = 1200.00f, ProductTypeId = 1 };
            var mappedResponse = new ProductPostResponse { Id = id, ProductName = "Laptop", Price = 1200.00f };

            _mockProductRepository.Setup(repo => repo.GetProduct(id)).ReturnsAsync(productToDelete);
            _mockProductRepository.Setup(repo => repo.DeleteProduct(productToDelete)).ReturnsAsync(productToDelete);
            _mockMapper.Setup(mapper => mapper.Map<ProductPostResponse>(productToDelete)).Returns(mappedResponse);

            var result = await _service.DeleteProduct(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("Laptop", result.ProductName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteProduct_ThrowsException_WhenProductNotFound()
        {
            int id = 99;
            _mockProductRepository.Setup(repo => repo.GetProduct(id)).ReturnsAsync((Product)null);

            await _service.DeleteProduct(id);
        }
    }
}
