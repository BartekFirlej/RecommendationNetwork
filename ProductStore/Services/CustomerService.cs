using ProductStore.Repositories;
using ProductStore.DTOs;
using ProductStore.Models;
using AutoMapper;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace ProductStore.Services
{
    public interface ICustomerService
    {
        public Task<ICollection<CustomerWithVoivodeshipResponse>> GetCustomers();
        public Task<CustomerWithVoivodeshipResponse> GetCustomerResponse(int id);
        public Task<Customer> GetCustomer(int id);
        public Task<CustomerResponse> PostCustomer(CustomerRequest customerToAdd);
        public Task<CustomerResponse> DeleteCustomer(int id);
        public Task<CustomerAuthenticationResult> CustomerAuthentication(CustomerAuthentication customerCredentials);
        public Task<CustomerAuthenticationResult> CustomerAuthenticationHash(CustomerAuthenticationHash customerCredentialsHash);
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
            customerToAdd.PIN = ComputeSha256Hash(customerToAdd.PIN);
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

        public async Task<CustomerAuthenticationResult> CustomerAuthentication(CustomerAuthentication customerCredentials)
        {
            customerCredentials.PIN = ComputeSha256Hash(customerCredentials.PIN);
            var customer = await _customerRepository.AuthenticateCustomer(customerCredentials);
            if (customer == null)
                return new CustomerAuthenticationResult { Authenticated = false, CustomerId = 0 };
            return new CustomerAuthenticationResult { Authenticated = true, CustomerId = customer.Id };
        }

        public async Task<CustomerAuthenticationResult> CustomerAuthenticationHash(CustomerAuthenticationHash customerCredentialsHash)
        {
            var customer = await _customerRepository.AuthenticateCustomerHash(customerCredentialsHash);
            if (customer == null)
                return new CustomerAuthenticationResult { Authenticated = false, CustomerId = 0 };
            return new CustomerAuthenticationResult { Authenticated = true, CustomerId = customer.Id };
        }

        public static string ComputeSha256Hash(string rawData)
        {  
            using (SHA256 sha256Hash = SHA256.Create())
            { 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}