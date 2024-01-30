using System.Net;
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
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PurchaseResponse>>("http://localhost:8082/purchases");
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<PurchaseResponse>();
            }
        }

        public async Task<PagedList<PurchaseResponse>> GetPurchasesPagedAsync(int index, int size)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PagedList<PurchaseResponse>>(String.Format("http://localhost:8082/purchases/paged/{0}/{1}",index, size));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new PagedList<PurchaseResponse>
                {
                    HasNextPage = false,
                    PagedItems = new List<PurchaseResponse>(),
                    PageIndex = index,
                    PageSize = size
                };
            }
        }

        public async Task<PagedList<PurchaseResponse>> GetCustomersPurchasesPagedAsync(int customerId,int index, int size)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PagedList<PurchaseResponse>>(String.Format("http://localhost:8082/purchases/customer/{0}/paged/{1}/{2}",customerId, index, size));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new PagedList<PurchaseResponse>
                {
                    HasNextPage = false,
                    PagedItems = new List<PurchaseResponse>(),
                    PageIndex = index,
                    PageSize = size
                };
            }
        }

        public async Task<PurchaseWithDetailsResponse> GetPurchaseDetailsAsync(int purchaseId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PurchaseWithDetailsResponse>(String.Format("http://localhost:8082/purchases/details/{0}", purchaseId));
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new PurchaseWithDetailsResponse();
            }
        }

        public async Task<PurchaseWithDetailsResponse> PostPurchaseDetailsAsync(PurchaseWithDetailsRequest purchaseToAdd)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/purchases/details", purchaseToAdd);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PurchaseWithDetailsResponse>();
            }
            else
            {
                throw new HttpRequestException($"Invalid response: {response.StatusCode}");
            }
        }
    }
}