using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseService
    {
        public Task<PurchaseResponse> AddPurchase(PurchaseRequest orderToAdd);
        public Task<List<PurchaseResponse>> GetPurchases();
        public Task<PurchaseResponse> GetPurchase(int id);
    }
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        public PurchaseService(IPurchaseRepository purchaseRepository, ICustomerService customerService, IProductService productService)
        {
            _purchaseRepository = purchaseRepository;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<PurchaseResponse> AddPurchase(PurchaseRequest orderToAdd)
        {
            await _customerService.GetCustomer(orderToAdd.CustomerId);
            if(orderToAdd.RecommenderId!=null)
            {
                await _customerService.GetCustomer((int)orderToAdd.RecommenderId);
            }
            return await _purchaseRepository.AddPurchase(orderToAdd);
        }

        public async Task<PurchaseResponse> GetPurchase(int id)
        {
            return await _purchaseRepository.GetPurchase(id);
        }

        public async Task<List<PurchaseResponse>> GetPurchases()
        {
            return await _purchaseRepository.GetPurchases();
        }
    }
}
