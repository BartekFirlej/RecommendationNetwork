using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;

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
            var mockProducts = new List<ProductResponse>
            {
                new ProductResponse { Id = 1, ProductName = "Product 1", Price = 100.00f, ProductTypeId = 1, ProductTypeName = "Electronics" },
                new ProductResponse { Id = 2, ProductName = "Product 2", Price = 50.00f, ProductTypeId = 2, ProductTypeName = "Books" }
            };
            _mockProductService.Setup(service => service.GetProducts()).ReturnsAsync(mockProducts);

            var result = await _controller.GetProducts();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProducts = okResult.Value as List<ProductResponse>;
            Assert.AreEqual(mockProducts.Count, returnedProducts.Count);
            Assert.AreEqual("Product 1", returnedProducts[0].ProductName);
            Assert.AreEqual(100.00f, returnedProducts[0].Price);
            Assert.AreEqual("Electronics", returnedProducts[0].ProductTypeName);
            Assert.AreEqual("Product 2", returnedProducts[1].ProductName);
            Assert.AreEqual(50.00f, returnedProducts[1].Price);
            Assert.AreEqual("Books", returnedProducts[1].ProductTypeName);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsNotFound_WhenExceptionThrown()
        {
            _mockProductService.Setup(service => service.GetProducts()).ThrowsAsync(new Exception());

            var result = await _controller.GetProducts();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetProduct_ReturnsOkWithProduct()
        {
            int id = 1;
            var mockProduct = new ProductResponse { Id = id, ProductName = "Product 1", Price = 100.00f, ProductTypeId = 1, ProductTypeName = "Electronics" };
            _mockProductService.Setup(service => service.GetProductResponse(id)).ReturnsAsync(mockProduct);

            var result = await _controller.GetProduct(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProduct = okResult.Value as ProductResponse;
            Assert.AreEqual(id, returnedProduct.Id);
            Assert.AreEqual("Product 1", returnedProduct.ProductName);
            Assert.AreEqual(100.00f, returnedProduct.Price);
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
        public async Task PostProduct_ReturnsCreatedAtActionWithProduct()
        {
            var productToAdd = new ProductRequest { ProductName = "New Product", Price = 200.00f, ProductTypeId = 3 };
            var addedProduct = new ProductPostResponse { Id = 3, ProductName = "New Product", Price = 200.00f, ProductTypeId = 3};
            _mockProductService.Setup(service => service.PostProduct(productToAdd)).ReturnsAsync(addedProduct);

            var result = await _controller.PostProduct(productToAdd);

            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            var returnedProduct = createdAtResult.Value as ProductPostResponse;
            Assert.AreEqual(3, returnedProduct.Id);
            Assert.AreEqual("New Product", returnedProduct.ProductName);
            Assert.AreEqual(200.00f, returnedProduct.Price);
        }

        [TestMethod]
        public async Task PostProduct_ReturnsNotFound_WhenProductCreationFails()
        {
            var productToAdd = new ProductRequest(); 
            _mockProductService.Setup(service => service.PostProduct(productToAdd)).ThrowsAsync(new Exception());

            var result = await _controller.PostProduct(productToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsOkWithProduct()
        {
            int id = 1;
            var deletedProduct = new ProductPostResponse { Id = id, ProductName = "Deleted Product", Price = 150.00f, ProductTypeId = 1 };
            _mockProductService.Setup(service => service.DeleteProduct(id)).ReturnsAsync(deletedProduct);

            var result = await _controller.DeleteProduct(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedProduct = okResult.Value as ProductPostResponse;
            Assert.AreEqual(id, returnedProduct.Id);
            Assert.AreEqual("Deleted Product", returnedProduct.ProductName);
            Assert.AreEqual(150.00f, returnedProduct.Price);
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
