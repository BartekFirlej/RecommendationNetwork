using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductStore.Controllers;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _mockCustomerService;
        private CustomerController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService.Object);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsOkWithCustomers()
        {
            var mockCustomers = new List<CustomerResponse>
    {
        new CustomerResponse { Id = 1, Name = "John", LastName = "Doe", Town = "Springfield" },
        new CustomerResponse { Id = 2, Name = "Jane", LastName = "Doe", Town = "Shelbyville" }
    };
            _mockCustomerService.Setup(service => service.GetCustomers()).ReturnsAsync(mockCustomers);

            var result = await _controller.GetCustomers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockCustomers, okResult.Value);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsNotFound_WhenExceptionThrown()
        {
            _mockCustomerService.Setup(service => service.GetCustomers()).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetCustomers();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsOkWithCustomer()
        {
            int id = 1;
            var mockCustomer = new CustomerResponse { Id = id, Name = "John", LastName = "Doe", Town = "Springfield" };
            _mockCustomerService.Setup(service => service.GetCustomerResponse(id)).ReturnsAsync(mockCustomer);

            var result = await _controller.GetCustomer(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockCustomerService.Setup(service => service.GetCustomerResponse(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetCustomer(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostCustomer_ReturnsOkWithCustomer()
        {
            var customerToAdd = new CustomerRequest { Name = "John", LastName = "Doe", Town = "Springfield" };
            var addedCustomer = new CustomerResponse { Id = 3, Name = "John", LastName = "Doe", Town = "Springfield" };
            _mockCustomerService.Setup(service => service.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);

            var result = await _controller.PostCustomer(customerToAdd);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(addedCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task PostCustomer_ReturnsNotFound_WhenExceptionThrown()
        {
            var customerToAdd = new CustomerRequest { Name = "John", LastName = "Doe", Town = "Springfield" };
            _mockCustomerService.Setup(service => service.PostCustomer(customerToAdd)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.PostCustomer(customerToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsOkWithCustomer()
        {
            int id = 1;
            var deletedCustomer = new CustomerResponse { Id = id, Name = "John", LastName = "Doe", Town = "Springfield" };
            _mockCustomerService.Setup(service => service.DeleteCustomer(id)).ReturnsAsync(deletedCustomer);

            var result = await _controller.DeleteCustomer(id);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(deletedCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsNotFound_WhenExceptionThrown()
        {
            int id = 1;
            _mockCustomerService.Setup(service => service.DeleteCustomer(id)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.DeleteCustomer(id);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
