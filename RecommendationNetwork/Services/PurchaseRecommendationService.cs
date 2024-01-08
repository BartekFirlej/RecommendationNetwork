using RecommendationNetwork.DTOs;
using RecommendationNetwork.Models;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseRecommendationService
    {
        public Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations();
        public Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendations(int customerId);
    }
    public class PurchaseRecommendationService : IPurchaseRecommendationService
    {
        private readonly IPurchaseRecommendationRepository _purchaseRecommendationRepository;
        private readonly ICustomerService _customerService;

        public PurchaseRecommendationService(IPurchaseRecommendationRepository purchaseRecommendationRepository, ICustomerService customerService)
        {
            _purchaseRecommendationRepository = purchaseRecommendationRepository;
            _customerService = customerService;
        }

        public async Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendations(int customerId)
        {
            await _customerService.GetCustomer(customerId);
            return await _purchaseRecommendationRepository.GetPurchasesCustomerRecommmendations(customerId);
        }

        public async Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations()
        {
            return await _purchaseRecommendationRepository.GetPurchasesCustomersRecommmendations();
        }
    }
}
