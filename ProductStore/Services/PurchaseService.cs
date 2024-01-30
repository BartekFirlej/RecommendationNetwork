using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IPurchaseService
    {
        public Task<ICollection<PurchaseResponse>> GetPurchases();
        public Task<PagedList<PurchaseResponse>> GetPurchasesPaged(int page, int size);
        public Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId);
        public Task<PagedList<PurchaseResponse>> GetCustomersPurchasesPaged(int customerId, int page, int size);
        public Task<PurchaseResponse> GetPurchaseResponse(int id);
        public Task<Purchase> GetPurchase(int id);
        public Task<PurchaseWithDetailsResponse> GetPurchaseWithDetails(int id);
        public Task<ICollection<PurchaseWithDetailsResponse>> GetPurchasesWithDetails();
        public Task<PurchaseResponse> DeletePurchase(int id);
        public Task<PurchaseResponse> PostPurchase(PurchaseRequest purchaseToAdd);
        public Task<PurchaseWithDetailsResponse> PostPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd);
    }
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseDetailService _purchaseDetailService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly RabbitMqPublisher _rabbitMqPublisher;

        public PurchaseService(IPurchaseRepository purchaseRepository, ICustomerService customerService, IMapper mapper, IPurchaseDetailService purchaseDetailService, RabbitMqPublisher rabbitMqPublisher)
        {
            _purchaseRepository = purchaseRepository;
            _customerService = customerService;
            _mapper = mapper;
            _purchaseDetailService = purchaseDetailService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<ICollection<PurchaseResponse>> GetPurchases()
        {
            var purchases = await _purchaseRepository.GetPurchases();
            if (!purchases.Any())
                throw new Exception("Not found any purchase.");
            return purchases;
        }


        public async Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId)
        {
            var purchases = await _purchaseRepository.GetCustomersPurchases(customerId);
            if (!purchases.Any())
                throw new Exception(String.Format("Not found any purchase for customer with id {0}.",customerId));
            return purchases;
        }

        public async Task<PagedList<PurchaseResponse>> GetPurchasesPaged(int page, int size)
        {
            var purchases = await _purchaseRepository.GetPurchasesPaged(page, size);
            if (!purchases.PagedItems.Any())
                throw new Exception("Not found any purchase.");
            return purchases;
        }

        public async Task<PagedList<PurchaseResponse>> GetCustomersPurchasesPaged(int customerId, int page, int size)
        {
            var purchases = await _purchaseRepository.GetCustomersPurchasesPaged(customerId, page, size);
            if (!purchases.PagedItems.Any())
                throw new Exception(String.Format("Not found any purchase for customer with id {0}.", customerId));
            return purchases;
        }
        public async Task<PurchaseResponse> GetPurchaseResponse(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseResponse(id);
            if (purchase == null)
                throw new Exception(String.Format("Not found purchase with id {0}.", id));
            return purchase;
        }

        public async Task<Purchase> GetPurchase(int id)
        {
            var purchase = await _purchaseRepository.GetPurchase(id);
            if (purchase == null)
                throw new Exception(String.Format("Not found purchase with id {0}.", id));
            return purchase;
        }

        public async Task<PurchaseWithDetailsResponse> GetPurchaseWithDetails(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseWithDetails(id);
            if (purchase == null)
                throw new Exception(String.Format("Not found purchase with id {0}.", id));
            return purchase;
        }

        public async Task<ICollection<PurchaseWithDetailsResponse>> GetPurchasesWithDetails()
        {
            var purchases = await _purchaseRepository.GetPurchasesWithDetails();
            if (!purchases.Any())
                throw new Exception(String.Format("Not found purchase."));
            return purchases;
        }

        public async Task<PurchaseResponse> DeletePurchase(int id)
        {
            var purchaseToDelete = await GetPurchase(id);
            await _purchaseDetailService.DeletePurchaseDetails(id);
            await _purchaseRepository.DeletePurchase(purchaseToDelete);
            return _mapper.Map<PurchaseResponse>(purchaseToDelete);
        }

        public async Task<PurchaseResponse> PostPurchase(PurchaseRequest purchaseToAdd)
        {
            if (purchaseToAdd.PurchaseDate == DateTime.MinValue)
                throw new Exception("Wrong date");
            await _customerService.GetCustomer(purchaseToAdd.CustomerId);
            if (purchaseToAdd.RecommenderId != null)
                await _customerService.GetCustomer((int)purchaseToAdd.RecommenderId);
            var addedPurchase = await _purchaseRepository.PostPurchase(purchaseToAdd);
            var addedPurchaseResponse = _mapper.Map<PurchaseResponse>(addedPurchase);
            _rabbitMqPublisher.PublishMessage(addedPurchaseResponse, "purchaseQueue");
            return addedPurchaseResponse;
        }

        public async Task<PurchaseWithDetailsResponse> PostPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd)
        {
            if (purchaseToAdd.PurchaseDate == DateTime.MinValue)
                throw new Exception("Wrong date");
            await _customerService.GetCustomer(purchaseToAdd.CustomerId);
            if (purchaseToAdd.RecommenderId != null)
                await _customerService.GetCustomer((int)purchaseToAdd.RecommenderId);
            var addedPurchase = await _purchaseRepository.PostPurchase(new PurchaseRequest { CustomerId = purchaseToAdd.CustomerId, PurchaseDate = purchaseToAdd.PurchaseDate, RecommenderId = purchaseToAdd.RecommenderId });
            foreach (var item in purchaseToAdd.Products)
            {
                if (item.Quantity <= 0)
                    throw new Exception(String.Format("Wrong quantity of product with id {0}.", item.ProductId));
                if (item.PriceForOnePiece <= 0)
                    throw new Exception(String.Format("Wrong price of product with id {0}.", item.ProductId));
                await _purchaseDetailService.AddPurchaseDetail(item, addedPurchase.Id);
            }
            var addedPurchaseWithDetailsResponse = await GetPurchaseWithDetails(addedPurchase.Id);
            _rabbitMqPublisher.PublishMessage(addedPurchaseWithDetailsResponse, "purchaseWithDetailsQueue");
            return addedPurchaseWithDetailsResponse;
        }
    }
}