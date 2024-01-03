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
    public class ProductTypeServiceTests
    {
        private Mock<IProductTypeRepository> _mockProductTypeRepository;
        private Mock<IMapper> _mockMapper;
        private ProductTypeService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockProductTypeRepository = new Mock<IProductTypeRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProductTypeService(_mockProductTypeRepository.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetProductTypes_ReturnsProductTypes_WhenFound()
        {
            var mockProductTypes = new List<ProductTypeResponse>
            {
                new ProductTypeResponse { Id = 1, Name = "Electronics" },
                new ProductTypeResponse { Id = 2, Name = "Books" }
            };
            _mockProductTypeRepository.Setup(repo => repo.GetProductTypes()).ReturnsAsync(mockProductTypes);

            var result = await _service.GetProductTypes();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(pt => pt.Name == "Electronics"));
            Assert.IsTrue(result.Any(pt => pt.Name == "Books"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any product type.")]
        public async Task GetProductTypes_ThrowsException_WhenNotFound()
        {
            _mockProductTypeRepository.Setup(repo => repo.GetProductTypes()).ReturnsAsync(new List<ProductTypeResponse>());

            await _service.GetProductTypes();
        }

        [TestMethod]
        public async Task GetProductTypeResponse_ReturnsProductType_WhenFound()
        {
            int id = 1;
            var mockProductType = new ProductTypeResponse { Id = id, Name = "Electronics" };
            _mockProductTypeRepository.Setup(repo => repo.GetProductTypeResponse(id)).ReturnsAsync(mockProductType);

            var result = await _service.GetProductTypeResponse(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Electronics", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetProductTypeResponse_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockProductTypeRepository.Setup(repo => repo.GetProductTypeResponse(id)).ReturnsAsync((ProductTypeResponse)null);

            await _service.GetProductTypeResponse(id);
        }

        [TestMethod]
        public async Task PostProductType_ReturnsAddedProductType_WhenSuccessful()
        {
            var productTypeToAdd = new ProductTypeRequest { Name = "Furniture" };
            var addedProductType = new ProductType { Id = 3, Name = "Furniture" };
            var mappedResponse = new ProductTypeResponse { Id = 3, Name = "Furniture" };

            _mockProductTypeRepository.Setup(repo => repo.PostProductType(productTypeToAdd)).ReturnsAsync(addedProductType);
            _mockMapper.Setup(mapper => mapper.Map<ProductTypeResponse>(addedProductType)).Returns(mappedResponse);

            var result = await _service.PostProductType(productTypeToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual("Furniture", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Napisales z malej litery.")]
        public async Task PostProductType_ThrowsException_WhenNameStartsWithLowercase()
        {
            var productTypeToAdd = new ProductTypeRequest { Name = "furniture" };

            await _service.PostProductType(productTypeToAdd);
        }

        [TestMethod]
        public async Task DeleteProductType_ReturnsDeletedProductType_WhenSuccessful()
        {
            int id = 1;
            var productTypeToDelete = new ProductType { Id = id, Name = "Electronics" };
            var mappedResponse = new ProductTypeResponse { Id = id, Name = "Electronics" };

            _mockProductTypeRepository.Setup(repo => repo.GetProductType(id)).ReturnsAsync(productTypeToDelete);
            _mockProductTypeRepository.Setup(repo => repo.DeleteProductType(productTypeToDelete)).ReturnsAsync(productTypeToDelete);
            _mockMapper.Setup(mapper => mapper.Map<ProductTypeResponse>(productTypeToDelete)).Returns(mappedResponse);

            var result = await _service.DeleteProductType(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Electronics", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteProductType_ThrowsException_WhenProductTypeNotFound()
        {
            int id = 99;
            _mockProductTypeRepository.Setup(repo => repo.GetProductType(id)).ReturnsAsync((ProductType)null);

            await _service.DeleteProductType(id);
        }
    }
}
