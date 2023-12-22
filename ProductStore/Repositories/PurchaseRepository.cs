using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<ICollection<PurchaseResponse>> GetPurchases();
        public Task<PurchaseResponse> GetPurchaseResponse(int id);
        public Task<Purchase> GetPurchase(int id);
        public Task<Purchase> DeletePurchase(Purchase purchaseToDelete);
        public Task<Purchase> PostPurchase(PurchaseRequest purchaseToAdd);
    }
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly StoreDbContext _dbContext;
        public PurchaseRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ICollection<PurchaseResponse>> GetPurchases()
        {
            return await _dbContext.Purchases
                .Select(p => new PurchaseResponse
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    CustomerId = p.CustomerId,
                    RecommenderId = p.RecommenderId
                })
                .ToListAsync();
        }

        public async Task<Purchase> GetPurchase(int id)
        {
            return await _dbContext.Purchases
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseResponse> GetPurchaseResponse(int id)
        {
            return await _dbContext.Purchases
                .Select(p => new PurchaseResponse
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    CustomerId = p.CustomerId,
                    RecommenderId = p.RecommenderId
                })
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Purchase> PostPurchase(PurchaseRequest purchaseToAdd)
        {
            var newPurchase = new Purchase
            {
                PurchaseDate = purchaseToAdd.PurchaseDate,
                CustomerId = purchaseToAdd.CustomerId,
                RecommenderId = purchaseToAdd.RecommenderId
            };
            await _dbContext.Purchases.AddAsync(newPurchase);
            await _dbContext.SaveChangesAsync();
            return newPurchase;
        }

        public async Task<Purchase> DeletePurchase(Purchase purchaseToDelete)
        {
            _dbContext.Purchases.Remove(purchaseToDelete);
            await _dbContext.SaveChangesAsync();
            return purchaseToDelete;
        }
    }
}
