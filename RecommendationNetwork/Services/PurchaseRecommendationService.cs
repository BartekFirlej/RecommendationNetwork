using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseRecommendationService
    {

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
    }
}
