using ProductStore.DTOs;
using ProductStore.Models;


namespace ProductStore.Repositories
{
    public interface IPurchaseProposalRepository
    {
        public Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals();
        public Task<PurchaseProposal> GetPurchaseProposal(int id);
        public Task<PurchaseProposalResponse> GetPurchaseProposalResponse(int id);
        public Task<PurchaseProposal> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd);
        public Task<PurchaseProposal> DeletePurchaseProposal(int id);

    }
    public class PurchaseProposalRepository : IPurchaseProposalRepository
    {
        private readonly StoreDbContext _dbContext;

        public Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals()
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseProposal> GetPurchaseProposal(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseProposalResponse> GetPurchaseProposalResponse(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseProposal> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseProposal> DeletePurchaseProposal(int id)
        {
            throw new NotImplementedException();
        }
    }
}
