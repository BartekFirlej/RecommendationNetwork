using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class PurchaseProposalsService
    {
        private readonly HttpClient _httpClient;

        public PurchaseProposalsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PurchaseProposalResponse> GetPurchaseProposalAsync(int customerId)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/purchase-proposals", new PurchaseProposalRequest{ CustomerId = customerId });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PurchaseProposalResponse>();
            }
            else
            {
                // Handle the error or throw an exception
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}