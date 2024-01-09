using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseService
    {
        public Task<PurchaseResponse> AddPurchase(PurchaseRequest purchaseToAdd);
        public Task<PurchaseResponse> AddPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd);
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

        public async Task<PurchaseResponse> AddPurchase(PurchaseRequest purchaseToAdd)
        {
            await _customerService.GetCustomer(purchaseToAdd.CustomerId);
            if (purchaseToAdd.RecommenderId != null)
            {
                await _customerService.GetCustomer((int)purchaseToAdd.RecommenderId);
            }
            return await _purchaseRepository.AddPurchase(purchaseToAdd);
        }

        public async Task<PurchaseResponse> AddPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd)
        {
            await _customerService.GetCustomer(purchaseToAdd.CustomerId);
            if (purchaseToAdd.RecommenderId != null)
            {
                await _customerService.GetCustomer((int)purchaseToAdd.RecommenderId);
            }
            foreach (var purchaseDetails in purchaseToAdd.PurchaseDetails)
            {
                await _productService.GetProduct(purchaseDetails.ProductId);
                if (purchaseDetails.Quantity <= 0)
                    throw new ValueMustBeGreaterThanZeroException("Quantity", purchaseDetails.ProductId);
                if (purchaseDetails.PriceForOnePiece <= 0)
                    throw new ValueMustBeGreaterThanZeroException("Price", purchaseDetails.ProductId);
            }
            return await _purchaseRepository.AddPurchaseWithDetails(purchaseToAdd);
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