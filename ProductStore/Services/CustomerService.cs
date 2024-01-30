using ProductStore.Repositories;
using ProductStore.DTOs;
using ProductStore.Models;
using AutoMapper;
using System.Text;
using System.Text.Json;


namespace ProductStore.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<CustomerWithVoivodeshipResponse>> GetCustomers();
        public Task<CustomerWithVoivodeshipResponse> GetCustomerResponse(int id);
        public Task<Customer> GetCustomer(int id);
        public Task<CustomerResponse> PostCustomer(CustomerRequest customerToAdd);
        public Task<CustomerResponse> DeleteCustomer(int id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly HttpClient _httpClient;
        private readonly RabbitMqPublisher _rabbitMqPublisher;

        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, RabbitMqPublisher rabbitMqPublisher, HttpClient httpClient)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _rabbitMqPublisher = rabbitMqPublisher;
            _httpClient = httpClient;
        }

        public async Task<ICollection<CustomerWithVoivodeshipResponse>> GetCustomers()
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

        public async Task<CustomerWithVoivodeshipResponse> GetCustomerResponse(int id)
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
            var addedCustomerResponse =  _mapper.Map<CustomerResponse>(addedCustomer);
            _rabbitMqPublisher.PublishMessage(addedCustomerResponse, "customerQueue");
            List<CartItem> emptyCart = new List<CartItem>();
            HttpContent content = new StringContent(JsonSerializer.Serialize(emptyCart), Encoding.UTF8, "application/json");
            await _httpClient.PostAsJsonAsync(String.Format("http://host.docker.internal:8082/cart/{0}", addedCustomerResponse.Id), emptyCart);
            return addedCustomerResponse;
        }

        public async Task<CustomerResponse> DeleteCustomer(int id)
        {
            var customerToDelete = await GetCustomer(id);
            await _customerRepository.DeleteCustomer(customerToDelete);
            await _httpClient.DeleteAsync(String.Format("http://host.docker.internal:8082/cart/{0}", customerToDelete.Id));
            return _mapper.Map<CustomerResponse>(customerToDelete);
        }
    }
}