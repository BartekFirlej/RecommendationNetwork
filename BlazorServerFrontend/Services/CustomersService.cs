using System.Net;
using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class CustomersService
    {
        private readonly HttpClient _httpClient;

        public CustomersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CustomerResponse>> GetCustomersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CustomerResponse>>("http://localhost:8082/customers");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<CustomerResponse>();
            }
        }

        public async Task<CustomerResponse> GetCustomerAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CustomerResponse>(String.Format("http://localhost:8082/customers/{0}",id));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new CustomerResponse();
            }
        }

        public async Task<CustomerResponse> PostCustomerAsync(CustomerRequest customerToAdd)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/customers", customerToAdd);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerResponse>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}