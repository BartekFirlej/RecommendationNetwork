using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IPurchaseProposalService
    {
        public Task<PurchaseProposalResponse> AddPurchaseProposal(PurchaseProposalRequest purchasePropsalRequest);
        public Task<List<PurchaseProposalResponse>> GetPurchaseProposals();
        public Task<PurchaseProposalResponse> GetPurchaseProposal(int customerId, int productId);

    }
    public class PurchaseProposalService : IPurchaseProposalService
    {
        private readonly IPurchaseProposalRepository _purchaseProposalRepository;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;


        public PurchaseProposalService(IPurchaseProposalRepository purchaseProposalRepository, ICustomerService customerService, IProductService productService, RabbitMqPublisher rabbitMqPublisher)
        {
            _purchaseProposalRepository = purchaseProposalRepository;
            _customerService = customerService;
            _productService = productService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<PurchaseProposalResponse> AddPurchaseProposal(PurchaseProposalRequest purchasePropsalRequest)
        {
            await _customerService.GetCustomer(purchasePropsalRequest.CustomerId);
            var products = await _productService.GetProducts();
            Random random = new Random();
            int randomIndex = random.Next(products.Count);
            var purchaseProposalResponse = await _purchaseProposalRepository.AddPurchaseProposal(purchasePropsalRequest, products[randomIndex].Id);
            _rabbitMqPublisher.PublishMessage(purchaseProposalResponse, "purchaseProposalQueue");
            return purchaseProposalResponse;
        }

        public async Task<PurchaseProposalResponse> GetPurchaseProposal(int customerId, int productId)
        {
            return await _purchaseProposalRepository.GetPurchaseProposal(customerId, productId);
        }

        public async Task<List<PurchaseProposalResponse>> GetPurchaseProposals()
        {
            return await _purchaseProposalRepository.GetPurchaseProposals();
        }
    }
}