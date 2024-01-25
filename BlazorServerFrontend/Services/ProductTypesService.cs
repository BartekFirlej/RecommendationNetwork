using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class ProductTypesService
    {
        private readonly HttpClient _httpClient;

        public ProductTypesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductTypeResponse>> GetProductTypesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductTypeResponse>>("http://localhost:8082/product-types");
        }

        public async Task<ProductTypeResponse> PostProductTypeAsync(ProductTypeRequest productTypeRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/product-types", productTypeRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductTypeResponse>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}
