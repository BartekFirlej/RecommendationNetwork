using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IPurchaseProposalService
    {
        public Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals();
        public Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals(int customerId);
        public Task<PurchaseProposal> GetPurchaseProposal(int id);
        public Task<PurchaseProposalResponse> GetPurchaseProposalResponse(int id);
        public Task<PurchaseProposalResponse> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd);
        public Task<PurchaseProposalResponse> DeletePurchaseProposal(int id);
    }
    public class PurchaseProposalService : IPurchaseProposalService
    {
        private readonly IPurchaseProposalRepository _purchaseProposalRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public PurchaseProposalService(IPurchaseProposalRepository purchaseProposalRepository, IMapper mapper, IProductService productService, ICustomerService customerService)
        {
            _purchaseProposalRepository = purchaseProposalRepository;
            _mapper = mapper;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals()
        {
            var purchaseProposals = await _purchaseProposalRepository.GetPurchaseProposals();
            if (!purchaseProposals.Any())
                throw new Exception("Not found any purachase proposals.");
            return purchaseProposals;
        }

        public async Task<PurchaseProposal> GetPurchaseProposal(int id)
        {
            var purchaseProposal = await _purchaseProposalRepository.GetPurchaseProposal(id);
            if (purchaseProposal == null)
                throw new Exception(String.Format("Not found purachase proposal with id {0}.", id));
            return purchaseProposal;
        }

        public async Task<PurchaseProposalResponse> GetPurchaseProposalResponse(int id)
        {
            var purchaseProposal = await _purchaseProposalRepository.GetPurchaseProposalResponse(id);
            if (purchaseProposal == null)
                throw new Exception(String.Format("Not found purachase proposal with id {0}.", id));
            return purchaseProposal;
        }

        public async Task<PurchaseProposalResponse> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd)
        {
            await _customerService.GetCustomer(purchaseProposalToAdd.CustomerId);
            await _productService.GetProduct(purchaseProposalToAdd.ProductId);
            var addedPurchaseProposal = await _purchaseProposalRepository.PostPurchaseProposal(purchaseProposalToAdd);
            return _mapper.Map<PurchaseProposalResponse>(addedPurchaseProposal);
        }

        public async Task<PurchaseProposalResponse> DeletePurchaseProposal(int id)
        {
            var purchaseProposalToDelete = await GetPurchaseProposal(id);
            var deletedProposal = await _purchaseProposalRepository.DeletePurchaseProposal(purchaseProposalToDelete);
            return _mapper.Map<PurchaseProposalResponse>(deletedProposal);
        }

        public async Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals(int customerId)
        {
            var purchaseProposals = await _purchaseProposalRepository.GetPurchaseProposals(customerId);
            if (!purchaseProposals.Any())
                throw new Exception("Not found any purachase proposals.");
            return purchaseProposals;
        }
    }
}