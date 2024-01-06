using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseDetailService
    {
        public Task<PurchaseIdDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetailToAdd);
        public Task<List<PurchaseIdDetailResponse>> GetPurchaseDetails();
        public Task<PurchaseIdDetailResponse> GetPurchaseDetail(int id);
    }
    public class PurchaseDetailService : IPurchaseDetailService
    {
        private readonly IPurchaseDetailRepository _purchaseDetailRepository;
        private readonly IProductService _productService;
        private readonly IPurchaseService _purchaseService;

        public PurchaseDetailService(IPurchaseDetailRepository purchaseDetailRepository, IProductService productService, IPurchaseService purchaseService)
        {
            _purchaseDetailRepository = purchaseDetailRepository;
            _productService = productService;
            _purchaseService = purchaseService;
        }

        public async Task<PurchaseIdDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetailToAdd)
        {
            if (purchaseDetailToAdd.Quantity <= 0)
                throw new ValueMustBeGreaterThanZeroException("Quantity", purchaseDetailToAdd.ProductId);
            if (purchaseDetailToAdd.PriceForOnePiece <= 0)
                throw new ValueMustBeGreaterThanZeroException("Price", purchaseDetailToAdd.ProductId);
            await _purchaseService.GetPurchase(purchaseDetailToAdd.PurchaseId);
            await _productService.GetProduct(purchaseDetailToAdd.ProductId);
            return await _purchaseDetailRepository.AddPurchaseDetail(purchaseDetailToAdd);
        }

        public async Task<PurchaseIdDetailResponse> GetPurchaseDetail(int id)
        {
            return await _purchaseDetailRepository.GetPurchaseDetail(id);
        }

        public async Task<List<PurchaseIdDetailResponse>> GetPurchaseDetails()
        {
            return await _purchaseDetailRepository.GetPurchaseDetails();
        }
    }
}
