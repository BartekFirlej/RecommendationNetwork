using BlazorServerFrontend.DTOs;
using System.Net;

namespace BlazorServerFrontend.Services
{
    public class VoivodeshipService
    {
        private readonly HttpClient _httpClient;

        public VoivodeshipService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VoivodeshipResponse>> GetVoivodeshipsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VoivodeshipResponse>>("http://localhost:8082/voivodeships");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<VoivodeshipResponse>();
            }
        }
    }
}
