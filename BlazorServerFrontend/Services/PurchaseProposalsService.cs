using System.Net;
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

        public async Task<PurchaseProposalResponse> GetPurchaseProposalAsync(PurchaseProposalRequest purchaseProposalRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/purchase-proposals", purchaseProposalRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PurchaseProposalResponse>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }

        public async Task<List<PurchaseProposalResponse>> GetCustomerPurchaseProposalsAsync(int customerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PurchaseProposalResponse>>(String.Format("http://localhost:8082/purchase-proposals/customers/{0}", customerId));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<PurchaseProposalResponse>();
            }
        }
    }
}