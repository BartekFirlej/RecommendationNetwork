using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreTests.Services
{

    [TestClass]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private Mock<IMapper> _mockMapper;
        private CustomerService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CustomerService(_mockCustomerRepository.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsCustomerList_WhenFound()
        {
            var mockCustomers = new List<CustomerResponse>
            {
                new CustomerResponse { Id = 1, Name = "John Doe" },
                new CustomerResponse { Id = 2, Name = "Jane Doe" }
            };
            _mockCustomerRepository.Setup(repo => repo.GetCustomers()).ReturnsAsync(mockCustomers);

            var result = await _service.GetCustomers();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Name == "John Doe"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not found any customer.")]
        public async Task GetCustomers_ThrowsException_WhenNotFound()
        {
            _mockCustomerRepository.Setup(repo => repo.GetCustomers()).ReturnsAsync(new List<CustomerResponse>());

            await _service.GetCustomers();
        }

        [TestMethod]
        public async Task GetCustomerResponse_ReturnsCustomer_WhenFound()
        {
            int id = 1;
            var mockCustomer = new CustomerResponse { Id = id, Name = "John Doe" };
            _mockCustomerRepository.Setup(repo => repo.GetCustomerResponse(id)).ReturnsAsync(mockCustomer);

            var result = await _service.GetCustomerResponse(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetCustomerResponse_ThrowsException_WhenNotFound()
        {
            int id = 99;
            _mockCustomerRepository.Setup(repo => repo.GetCustomerResponse(id)).ReturnsAsync((CustomerResponse)null);

            await _service.GetCustomerResponse(id);
        }

        [TestMethod]
        public async Task PostCustomer_ReturnsAddedCustomer_WhenSuccessful()
        {
            var customerToAdd = new CustomerRequest { Name = "New Customer" };
            var addedCustomer = new Customer { Id = 3, Name = "New Customer" };
            var mappedResponse = new CustomerResponse { Id = 3, Name = "New Customer" };

            _mockCustomerRepository.Setup(repo => repo.PostCustomer(It.IsAny<CustomerRequest>())).ReturnsAsync(addedCustomer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerResponse>(addedCustomer)).Returns(mappedResponse);

            var result = await _service.PostCustomer(customerToAdd);

            Assert.IsNotNull(result);
            Assert.AreEqual("New Customer", result.Name);
        }

        [TestMethod]
        public async Task DeleteCustomer_ReturnsDeletedCustomer_WhenSuccessful()
        {
            int id = 1;
            var customerToDelete = new Customer { Id = id, Name = "John Doe" };
            var mappedResponse = new CustomerResponse { Id = id, Name = "John Doe" };

            _mockCustomerRepository.Setup(repo => repo.GetCustomer(id)).ReturnsAsync(customerToDelete);
            _mockCustomerRepository.Setup(repo => repo.DeleteCustomer(customerToDelete)).ReturnsAsync(customerToDelete);
            _mockMapper.Setup(mapper => mapper.Map<CustomerResponse>(customerToDelete)).Returns(mappedResponse);

            var result = await _service.DeleteCustomer(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteCustomer_ThrowsException_WhenCustomerNotFound()
        {
            int id = 99;
            _mockCustomerRepository.Setup(repo => repo.GetCustomer(id)).ReturnsAsync((Customer)null);

            await _service.DeleteCustomer(id);
        }

    }

}
