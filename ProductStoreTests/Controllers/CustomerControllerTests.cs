using ProductStore.Controllers;
using ProductStore.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;

namespace ProductStoreTests.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _mockCustomerService;
        private CustomerController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService.Object);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsOkWithCustomers()
        {
            var mockCustomers = new List<CustomerResponse>();
            _mockCustomerService.Setup(s => s.GetCustomers()).ReturnsAsync(mockCustomers);

            var result = await _controller.GetCustomers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsNotFound_WhenCustomersNotFound()
        {
            _mockCustomerService.Setup(s => s.GetCustomers()).Throws(new Exception());

            var result = await _controller.GetCustomers();

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsOkWithCustomer_WhenCustomerExists()
        {
            int customerId = 1;
            var mockCustomer = new CustomerResponse();
            _mockCustomerService.Setup(s => s.GetCustomerResponse(customerId)).ReturnsAsync(mockCustomer);

            var result = await _controller.GetCustomer(customerId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            int customerId = 1;
            _mockCustomerService.Setup(s => s.GetCustomerResponse(customerId)).Throws(new Exception());

            var result = await _controller.GetCustomer(customerId);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostCustomer_ReturnsOkWithCustomer_WhenCustomerIsCreated()
        {
            var customerToAdd = new CustomerRequest();
            var addedCustomer = new CustomerResponse(); 
            _mockCustomerService.Setup(s => s.PostCustomer(customerToAdd)).ReturnsAsync(addedCustomer);

            var result = await _controller.PostCustomer(customerToAdd);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(addedCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task PostCustomer_ReturnsNotFound_WhenCustomerCreationFails()
        {
            var customerToAdd = new CustomerRequest();
            _mockCustomerService.Setup(s => s.PostCustomer(customerToAdd)).Throws(new Exception());

            var result = await _controller.PostCustomer(customerToAdd);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsOkWithCustomer_WhenCustomerIsDeleted()
        {
            int customerId = 1;
            var mockCustomer = new CustomerResponse();
            _mockCustomerService.Setup(s => s.DeleteCustomer(customerId)).ReturnsAsync(mockCustomer);

            var result = await _controller.DeleteCustomer(customerId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mockCustomer, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            int customerId = 1;
            _mockCustomerService.Setup(s => s.DeleteCustomer(customerId)).Throws(new Exception());

            var result = await _controller.DeleteCustomer(customerId);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }
}
