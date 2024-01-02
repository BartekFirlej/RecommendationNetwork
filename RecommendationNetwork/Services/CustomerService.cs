using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface ICustomerService
    {
        public Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd);
        public Task<List<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse> GetCustomer(int id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVoivodeshipService _voivodeshipService;
        public CustomerService(ICustomerRepository customerRepository, IVoivodeshipService voivodeshipService, RabbitMqConsumer rabbitMqConsumer)
        {
            _customerRepository = customerRepository;
            _voivodeshipService = voivodeshipService;
        }
        public async Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd)
        {
            await _voivodeshipService.GetVoivodeship(customerToAdd.VoivodeshipId);
            return await _customerRepository.AddCustomer(customerToAdd);
        }

        public async Task<List<CustomerResponse>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }

        public async Task<CustomerResponse> GetCustomer(int id)
        {
            return await _customerRepository.GetCustomer(id);
        }
    }
}
