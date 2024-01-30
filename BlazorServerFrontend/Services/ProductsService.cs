using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ProductResponse>>("http://localhost:8082/products");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<ProductResponse>();
            }
        }

        public async Task<PagedList<ProductResponse>> GetProductsPagedAsync(int index, int size)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PagedList<ProductResponse>>(String.Format("http://localhost:8082/products/{0}/{1}", index, size));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new PagedList<ProductResponse>() { 
                    HasNextPage = false, 
                    PagedItems = new List<ProductResponse>() , 
                    PageIndex = index, 
                    PageSize = size};
            }
        }

        public async Task<List<ProductResponse>> GetProductsIdsAsync(IdsListDTO ids)
        {

            var jsonRequest = JsonSerializer.Serialize(ids);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:8082/products/byids", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
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

        public async Task<ProductResponse> PostFakeProductAsync()
        {
            var response = await _httpClient.PostAsync("http://localhost:8082/products/api", null);

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