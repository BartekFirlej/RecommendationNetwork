using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface ICustomerRecommendationService
    {
        public Task<List<CustomerRecommendationResponse>> GetCustomersCustomersRecommmendations();
        public Task<CustomerRecommendationResponse> GetCustomersCustomerRecommmendation(int customerId);
    }
    public class CustomerRecommendationService : ICustomerRecommendationService
    {
        private readonly ICustomerRecommendationRepository _customerRecommendationRepository;
        private readonly ICustomerService _customerService;

        public CustomerRecommendationService(ICustomerRecommendationRepository customerRecommendationRepository, ICustomerService customerService)
        {
            _customerRecommendationRepository = customerRecommendationRepository;
            _customerService = customerService;
        }

        public async Task<CustomerRecommendationResponse> GetCustomersCustomerRecommmendation(int customerId)
        {
            await _customerService.GetCustomer(customerId);
            return await _customerRecommendationRepository.GetCustomersCustomerRecommmendation(customerId);
        }

        public async Task<List<CustomerRecommendationResponse>> GetCustomersCustomersRecommmendations()
        {
            return await _customerRecommendationRepository.GetCustomersCustomersRecommmendations();
        }
    }
}
