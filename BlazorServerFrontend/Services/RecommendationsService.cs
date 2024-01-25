﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            return await _httpClient.GetFromJsonAsync<List<RecommendationResponse>>("http://localhost:8082/recommmendations/customers");
        }

        public async Task<List<RecommendationResponse>> GetPurchasesRecommendationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RecommendationResponse>>("http://localhost:8082/recommmendations/purchases");
        }
    }
}