using Microsoft.EntityFrameworkCore;
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
        public Task<PurchaseProposal> DeletePurchaseProposal(PurchaseProposal purchaseProposalToDelete);

    }
    public class PurchaseProposalRepository : IPurchaseProposalRepository
    {
        private readonly StoreDbContext _dbContext;

        public async Task<ICollection<PurchaseProposalResponse>> GetPurchaseProposals()
        {
            return await _dbContext.PurchaseProposals
                .Select(p => new PurchaseProposalResponse
                {
                    Id = p.Id,
                    Date = p.Date,
                    CustomerId = p.CustomerId,
                    ProductId = p.ProductId
                }).ToListAsync();
        }

        public async Task<PurchaseProposal> GetPurchaseProposal(int id)
        {
            return await _dbContext.PurchaseProposals
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseProposalResponse> GetPurchaseProposalResponse(int id)
        {
            return await _dbContext.PurchaseProposals
                .Select(p => new PurchaseProposalResponse
                {
                    Id = p.Id,
                    Date = p.Date,
                    CustomerId = p.CustomerId,
                    ProductId = p.ProductId
                })
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseProposal> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd)
        {
            var newPurchaseProposal = new PurchaseProposal
            {
                CustomerId = purchaseProposalToAdd.CustomerId,
                ProductId = purchaseProposalToAdd.ProductId,
                Date = purchaseProposalToAdd.Date
            };
            await _dbContext.PurchaseProposals.AddAsync(newPurchaseProposal);
            await _dbContext.SaveChangesAsync();
            return newPurchaseProposal;
        }

        public async Task<PurchaseProposal> DeletePurchaseProposal(PurchaseProposal purchaseProposalToDelete)
        {
            _dbContext.Remove(purchaseProposalToDelete);
            await _dbContext.SaveChangesAsync();
            return purchaseProposalToDelete;
        }
    }
}
