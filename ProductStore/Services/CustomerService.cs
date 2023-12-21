using ProductStore.Repositories;
using ProductStore.DTOs;
using AutoMapper;

namespace ProductStore.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse> GetCustomer(int id);
        public Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd);
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

        public async Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd)
        {
            var addedCustomer =  await _customerRepository.AddCustomer(customerToAdd);
            return _mapper.Map<CustomerResponse>(addedCustomer);
        }

        public async Task<CustomerResponse> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
                throw new Exception(String.Format("Not found customer with id {0}.", id));
            return customer;
        }

        public async Task<ICollection<CustomerResponse>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            if (customers.Count == 0)
                throw new Exception("Not found any customer.");
            return customers;
        }
    }
}
