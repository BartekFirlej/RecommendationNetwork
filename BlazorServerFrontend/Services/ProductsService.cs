using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class ProductsService
    {
        private readonly HttpClient _httpClient;

        public ProductsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductResponse>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductResponse>>("http://localhost:8082/products");
        }

        public async Task<ProductResponse> PostProductAsync(ProductRequest productRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/products", productRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductResponse>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}