using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            return await _httpClient.GetFromJsonAsync<List<CustomerResponse>>("http://localhost:8082/customers");
        }
    }
}