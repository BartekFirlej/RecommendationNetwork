using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface ICustomerRepository
    {
        public Task<ICollection<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse> GetCustomerResponse(int id);
        public Task<Customer> GetCustomer(int id);
        public Task<Customer> DeleteCustomer(Customer customerToDelete);
        public Task<Customer> AddCustomer(CustomerRequest customerToAdd);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StoreDbContext _dbContext;
        public CustomerRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<CustomerResponse>> GetCustomers()
        {
            return await _dbContext.Customers
                .Select(c => new CustomerResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    LastName = c.LastName,
                    Country = c.Country,
                    ZipCode = c.ZipCode,
                    Street = c.Street,
                    Town = c.Town,
                    VoivodeshipId = c.VoivodeshipId
                })
                .ToListAsync();
        }

        public async Task<CustomerResponse> GetCustomerResponse(int id)
        {
            return await _dbContext.Customers
                .Select(c => new CustomerResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    LastName = c.LastName,
                    Country = c.Country,
                    ZipCode = c.ZipCode,
                    Street = c.Street,
                    Town = c.Town,
                    VoivodeshipId = c.VoivodeshipId
                })
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> AddCustomer(CustomerRequest customerToAdd)
        {
            var customer = new Customer
            {
                Name = customerToAdd.Name,
                LastName = customerToAdd.LastName,
                Country = customerToAdd.Country,
                ZipCode = customerToAdd.ZipCode,
                Street = customerToAdd.Street,
                Town = customerToAdd.Town,
                VoivodeshipId = customerToAdd.VoivodeshipId
            };
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> DeleteCustomer(Customer customerToDelete)
        {
            _dbContext.Customers.Remove(customerToDelete);
            await _dbContext.SaveChangesAsync();
            return customerToDelete;
        }
    }
}
