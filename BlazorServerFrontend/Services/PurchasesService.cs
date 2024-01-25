using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class PurchasesService
    {
        private readonly HttpClient _httpClient;

        public PurchasesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PurchaseResponse>> GetPurchasesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PurchaseResponse>>("http://localhost:8082/purchases");
        }
    }
}