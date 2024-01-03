using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreTests.Repositories
{


    [TestClass]
    public class CustomerRepositoryTests
    {
        private StoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;

            var dbContext = new StoreDbContext(options);
            return dbContext;
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsAllCustomers()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship
            {
                Id = 1,
                Name = "Mazowieckie"
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Town = "Springfield",
                ZipCode = "12345",
                Street = "Main Street",
                Country = "CountryA",
                VoivodeshipId = 1,
                RecommenderId = null
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 2,
                Name = "Jane",
                LastName = "Smith",
                Town = "Shelbyville",
                ZipCode = "54321",
                Street = "Second Street",
                Country = "CountryB",
                VoivodeshipId = 2,
                RecommenderId = 1
            });
            dbContext.SaveChanges();
            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomers();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetCustomers_ReturnsEmpty_WhenNoCustomersExist()
        {
            using var dbContext = CreateDbContext();

            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomers();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetCustomerResponse_ReturnsCustomer_WhenExists()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship
            {
                Id = 1,
                Name = "Mazowieckie"
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Town = "Springfield",
                ZipCode = "12345",
                Street = "Main Street",
                Country = "CountryA",
                VoivodeshipId = 1,
                RecommenderId = null
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 2,
                Name = "Jane",
                LastName = "Smith",
                Town = "Shelbyville",
                ZipCode = "54321",
                Street = "Second Street",
                Country = "CountryB",
                VoivodeshipId = 2,
                RecommenderId = 1
            });
            dbContext.SaveChanges();
            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomerResponse(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.Name);
        }

        [TestMethod]
        public async Task GetCustomerResponse_ReturnsNull_WhenCustomerDoesNotExist()
        {
            using var dbContext = CreateDbContext();
            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomerResponse(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCustomer_ReturnsCustomer()
        {
            using var dbContext = CreateDbContext();
            dbContext.Voivodeships.Add(new Voivodeship
            {
                Id = 1,
                Name = "Mazowieckie"
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Town = "Springfield",
                ZipCode = "12345",
                Street = "Main Street",
                Country = "CountryA",
                VoivodeshipId = 1,
                RecommenderId = null
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 2,
                Name = "Jane",
                LastName = "Smith",
                Town = "Shelbyville",
                ZipCode = "54321",
                Street = "Second Street",
                Country = "CountryB",
                VoivodeshipId = 2,
                RecommenderId = 1
            });
            dbContext.SaveChanges();
            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomer(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.Name);
        }


        [TestMethod]
        public async Task GetCustomer_ReturnsNull_WhenCustomerDoesNotExist()
        {
            using var dbContext = CreateDbContext();
            var repository = new CustomerRepository(dbContext);

            var result = await repository.GetCustomer(99);

            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task PostCustomer_AddsNewCustomer()
        {
            using var dbContext = CreateDbContext();
            var repository = new CustomerRepository(dbContext);
            var newCustomer = new CustomerRequest
            {
                Name = "Jane",
                LastName = "Doe",
                Town = "Springfield",
                ZipCode = "54321",
                Street = "Second Street",
                Country = "CountryB",
            };

            var result = await repository.PostCustomer(newCustomer);

            var addedCustomer = dbContext.Customers.FirstOrDefault(c => c.Name == "Jane" && c.LastName == "Doe");
            Assert.IsNotNull(addedCustomer);
            Assert.AreEqual("Springfield", addedCustomer.Town);
        }

        [TestMethod]
        public async Task DeleteCustomer_RemovesCustomer_WhenExists()
        {
            using var dbContext = CreateDbContext();

            dbContext.Voivodeships.Add(new Voivodeship
            {
                Id = 1,
                Name = "Mazowieckie"
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Town = "Springfield",
                ZipCode = "12345",
                Street = "Main Street",
                Country = "CountryA",
                VoivodeshipId = 1,
                RecommenderId = null
            });
            dbContext.Customers.Add(new Customer
            {
                Id = 2,
                Name = "Jane",
                LastName = "Smith",
                Town = "Shelbyville",
                ZipCode = "54321",
                Street = "Second Street",
                Country = "CountryB",
                VoivodeshipId = 2,
                RecommenderId = 1
            });
            dbContext.SaveChanges();
            var repository = new CustomerRepository(dbContext);
            var customerToDelete = dbContext.Customers.First();

            await repository.DeleteCustomer(customerToDelete);

            Assert.IsFalse(dbContext.Customers.Any(c => c.Id == customerToDelete.Id));
        }

    }

}
