using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerAuthenticationResult> AuthCustomer(CustomerAuthentication customer)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/customers/auth", customer);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerAuthenticationResult>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }

        public async Task<CustomerAuthenticationResult> AuthCustomerHash(CustomerAuthenticationHash customer)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/customers/auth/hash", customer);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerAuthenticationResult>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}
