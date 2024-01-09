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
        public Task<ICollection<PurchaseDetailResponse>> GetPurchaseDetails();
        public Task<ICollection<PurchaseDetail>> GetPurchaseDetails(int orderId);
        public Task<PurchaseDetailResponse> GetPurchaseDetailResponse(int id);
        public Task<PurchaseDetail> GetPurchaseDetail(int id);
        public Task<PurchaseDetailResponse> DeletePurchaseDetail(int id);
        public Task<ICollection<PurchaseDetailResponse>> DeletePurchaseDetails(int orderId);
    }
    public class PurchaseDetailService : IPurchaseDetailService
    {
        private readonly IProductService _productService;
        private readonly IPurchaseDetailRepository _purchaseDetailRepository;
        private readonly IMapper _mapper;
        private readonly RabbitMqPublisher _rabbitMqPublisher;

        public PurchaseDetailService(IPurchaseDetailRepository purchaseDetailRepository, IProductService productService, IMapper mapper, RabbitMqPublisher rabbitMqPublisher)
        {
            _productService = productService;
            _purchaseDetailRepository = purchaseDetailRepository;
            _mapper = mapper;
            _rabbitMqPublisher = rabbitMqPublisher;
        }
        public async Task<ICollection<PurchaseDetailResponse>> GetPurchaseDetails()
        {
            var purchaseDetails = await _purchaseDetailRepository.GetPurchaseDetails();
            if (!purchaseDetails.Any())
                throw new Exception("Not found any purchase details.");
            return purchaseDetails;
        }

        public async Task<PurchaseDetail> GetPurchaseDetail(int id)
        {
            var purchaseDetail = await _purchaseDetailRepository.GetPurchaseDetail(id);
            if (purchaseDetail == null)
                throw new Exception(String.Format("Not found any purchase detail with id {0}.", id));
            return purchaseDetail;
        }

        public async Task<PurchaseDetailResponse> GetPurchaseDetailResponse(int id)
        {
            var purchaseDetail = await _purchaseDetailRepository.GetPurchaseDetailResponse(id);
            if (purchaseDetail == null)
                throw new Exception(String.Format("Not found any purchase detail with id {0}.", id));
            return purchaseDetail;
        }

        public async Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseDetailRequest purchaseDetail, int orderId)
        {
            await _productService.GetProduct(purchaseDetail.ProductId);
            var addedDetail = await _purchaseDetailRepository.AddPurchaseDetail(purchaseDetail, orderId);
            var addedDetailResponse = _mapper.Map<PurchaseDetailResponse>(addedDetail);
            _rabbitMqPublisher.PublishMessage(addedDetailResponse, "purchaseDetailQueue");
            return addedDetailResponse;
        }

        public async Task<PurchaseDetailResponse> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetail)
        {
            if (purchaseDetail.Quantity <= 0)
                throw new Exception(String.Format("Wrong quantity of product with id {0}.", purchaseDetail.ProductId));
            if (purchaseDetail.PriceForOnePiece <= 0)
                throw new Exception(String.Format("Wrong price of product with id {0}.", purchaseDetail.ProductId));
            await _productService.GetProduct(purchaseDetail.ProductId);
            var addedDetail = await _purchaseDetailRepository.AddPurchaseDetail(purchaseDetail);
            var addedDetailResponse = _mapper.Map<PurchaseDetailResponse>(addedDetail);
            _rabbitMqPublisher.PublishMessage(addedDetailResponse, "purchaseDetailQueue");
            return addedDetailResponse;
        }

        public async Task<PurchaseDetailResponse> DeletePurchaseDetail(int id)
        {
            var purchaseDetailToDelete = await GetPurchaseDetail(id);
            var deletedPurchaseDetail = await _purchaseDetailRepository.DeletePurchaseDetail(purchaseDetailToDelete);
            return _mapper.Map<PurchaseDetailResponse>(deletedPurchaseDetail);
        }

        public async Task<ICollection<PurchaseDetail>> GetPurchaseDetails(int orderId)
        {
            return await _purchaseDetailRepository.GetPurchaseDetails(orderId);
        }

        public async Task<ICollection<PurchaseDetailResponse>> DeletePurchaseDetails(int orderId)
        {
            var purchaseDetailToDelete = await GetPurchaseDetails(orderId);
            var deletedPurchaseDetail = await _purchaseDetailRepository.DeletePurchaseDetail(purchaseDetailToDelete);
            return _mapper.Map<List<PurchaseDetailResponse>>(deletedPurchaseDetail);
        }
    }
}