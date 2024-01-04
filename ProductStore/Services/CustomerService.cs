using ProductStore.Repositories;
using ProductStore.DTOs;
using ProductStore.Models;
using AutoMapper;

namespace ProductStore.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse> GetCustomerResponse(int id);
        public Task<Customer> GetCustomer(int id);
        public Task<CustomerResponse> PostCustomer(CustomerRequest customerToAdd);
        public Task<CustomerResponse> DeleteCustomer(int id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CustomerResponse>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            if (customers.Count == 0)
                throw new Exception("Not found any customer.");
            return customers;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
                throw new Exception(String.Format("Not found customer with id {0}.", id));
            return customer;
        }

        public async Task<CustomerResponse> GetCustomerResponse(int id)
        {
            var customer = await _customerRepository.GetCustomerResponse(id);
            if (customer == null)
                throw new Exception(String.Format("Not found customer with id {0}.", id));
            return customer;
        }

        public async Task<CustomerResponse> PostCustomer(CustomerRequest customerToAdd)
        {
            if (customerToAdd.RecommenderId != null)
                await GetCustomer((int)customerToAdd.RecommenderId);
            var addedCustomer = await _customerRepository.PostCustomer(customerToAdd);
            return _mapper.Map<CustomerResponse>(addedCustomer);
        }

        public async Task<CustomerResponse> DeleteCustomer(int id)
        {
            var customerToDelete = await GetCustomer(id);
            await _customerRepository.DeleteCustomer(customerToDelete);
            return _mapper.Map<CustomerResponse>(customerToDelete);
        }
    }
}
