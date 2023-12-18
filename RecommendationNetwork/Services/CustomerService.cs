using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface ICustomerService
    {
        public Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd);
        public Task<List<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse> GetCustomer(string pesel);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd)
        {
            return await _customerRepository.AddCustomer(customerToAdd);
        }

        public async Task<List<CustomerResponse>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }

        public async Task<CustomerResponse> GetCustomer(string pesel)
        {
            return await _customerRepository.GetCustomer(pesel);
        }
    }
}
