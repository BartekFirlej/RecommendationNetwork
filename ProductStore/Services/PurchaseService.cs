using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IPurchaseService
    {
        public Task<ICollection<PurchaseResponse>> GetPurchases();
        public Task<PurchaseResponse> GetPurchaseResponse(int id);
        public Task<Purchase> GetPurchase(int id);
        public Task<PurchaseResponse> DeletePurchase(int id);
        public Task<PurchaseResponse> PostPurchase(PurchaseRequest purchaseToAdd);
    }
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public PurchaseService(IPurchaseRepository purchaseRepository, ICustomerService customerService, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<ICollection<PurchaseResponse>> GetPurchases()
        {
            var purchases = await _purchaseRepository.GetPurchases();
            if (!purchases.Any())
                throw new Exception("Not found any purchase.");
            return purchases;
        }

        public async Task<PurchaseResponse> GetPurchaseResponse(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseResponse(id);
            if (purchase == null)
                throw new Exception(String.Format("Not found purchase with id {0}.",id));
            return purchase;
        }

        public async Task<Purchase> GetPurchase(int id)
        {
            var purchase = await _purchaseRepository.GetPurchase(id);
            if (purchase == null)
                throw new Exception(String.Format("Not found purchase with id {0}.", id));
            return purchase;
        }

        public async Task<PurchaseResponse> DeletePurchase(int id)
        {
            var purchaseToDelete = await GetPurchase(id);
            await _purchaseRepository.DeletePurchase(purchaseToDelete);
            return _mapper.Map<PurchaseResponse>(purchaseToDelete);
        }

        public async Task<PurchaseResponse> PostPurchase(PurchaseRequest purchaseToAdd)
        {
            await _customerService.GetCustomer(purchaseToAdd.CustomerId);
            if (purchaseToAdd.RecommenderId != null)
                await _customerService.GetCustomer((int)purchaseToAdd.RecommenderId);
            var addedPurchase = await _purchaseRepository.PostPurchase(purchaseToAdd);
            return _mapper.Map<PurchaseResponse>(addedPurchase);
        }
    }
}
