using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IPurchaseDetailService
    {
        public Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseDetailRequest purchaseDetail, int orderId);
        public Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetail);
    }
    public class PurchaseDetailService : IPurchaseDetailService
    {
        private readonly IProductService _productService;
        private readonly IPurchaseDetailRepository _purchaseDetailRepository;
        private readonly IMapper _mapper;
        
        public PurchaseDetailService(IPurchaseDetailRepository purchaseDetailRepository, IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _purchaseDetailRepository = purchaseDetailRepository;
            _mapper = mapper;
        }

        public async Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseDetailRequest purchaseDetail, int orderId)
        {
            await _productService.GetProduct(purchaseDetail.ProductId);
            var addedDetail = await _purchaseDetailRepository.AddPurchaseDetail(purchaseDetail, orderId);
            return _mapper.Map<PurchaseDetailResponse>(addedDetail);
        }

        public async Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetail)
        {
            await _productService.GetProduct(purchaseDetail.ProductId);
            var addedDetail = await _purchaseDetailRepository.AddPurchaseDetail(purchaseDetail);
            return _mapper.Map<PurchaseDetailResponse>(addedDetail);
        }
    }
}
