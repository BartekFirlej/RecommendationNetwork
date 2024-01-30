using System.Net;
using BlazorServerFrontend.DTOs;

namespace BlazorServerFrontend.Services
{
    public class RecommendationsService
    {
        private readonly HttpClient _httpClient;

        public RecommendationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RecommendationResponse>> GetCustomersRecommendationsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<RecommendationResponse>>("http://localhost:8082/recommmendations/customers");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<RecommendationResponse>();
            }
        }

        public async Task<List<RecommendationResponse>> GetPurchasesRecommendationsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<RecommendationResponse>>("http://localhost:8082/recommmendations/purchases");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<RecommendationResponse>();
            }
        }
    }
}