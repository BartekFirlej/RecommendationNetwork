using System.Net;
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
    }
}