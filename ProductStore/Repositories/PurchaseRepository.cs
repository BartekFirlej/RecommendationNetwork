using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<ICollection<PurchaseResponse>> GetPurchases();
        public Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId);
        public Task<PurchaseResponse> GetPurchaseResponse(int id);
        public Task<Purchase> GetPurchase(int id);
        public Task<Purchase> DeletePurchase(Purchase purchaseToDelete);
        public Task<Purchase> PostPurchase(PurchaseRequest purchaseToAdd);
        public Task<PurchaseWithDetailsResponse> GetPurchaseWithDetails(int id);
        public Task<ICollection<PurchaseWithDetailsResponse>> GetPurchasesWithDetails();
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
            return await _dbContext.PurchaseDetails
   .Include(p => p.Purchase)
   .GroupBy(p => p.Purchase.Id) // Group by PurchaseId
   .Select(group => new PurchaseResponse
   {
       Id = group.Key, // Use the PurchaseId as key
       PurchaseDate = group.First().Purchase.PurchaseDate, // Assuming all items in the group have the same PurchaseDate
       CustomerId = group.First().Purchase.CustomerId, // Assuming all items in the group have the same CustomerId
       RecommenderId = group.First().Purchase.RecommenderId, // Assuming all items in the group have the same RecommenderId
       Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity) // Correctly summing ProductPrice * Quantity for each item
   }).ToListAsync();
        }


        public async Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId)
        {
            return await _dbContext.PurchaseDetails
    .Include(p => p.Purchase)
    .GroupBy(p => p.Purchase.Id) // Group by PurchaseId
    .Select(group => new PurchaseResponse
    {
        Id = group.Key, // Use the PurchaseId as key
        PurchaseDate = group.First().Purchase.PurchaseDate, // Assuming all items in the group have the same PurchaseDate
        CustomerId = group.First().Purchase.CustomerId, // Assuming all items in the group have the same CustomerId
        RecommenderId = group.First().Purchase.RecommenderId, // Assuming all items in the group have the same RecommenderId
        Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity) // Correctly summing ProductPrice * Quantity for each item
    })
               .Where(p => p.CustomerId == customerId)
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
            return await _dbContext.PurchaseDetails
               .Include(p => p.Purchase)
               .GroupBy(p => p.Purchase.Id)
               .Select(group => new PurchaseResponse
               {
                   Id = group.Key,
                   PurchaseDate = group.First().Purchase.PurchaseDate,
                   CustomerId = group.First().Purchase.CustomerId,
                   RecommenderId = group.First().Purchase.RecommenderId,
                   Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity)
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

        public Task<PurchaseWithDetailsResponse> GetPurchaseWithDetails(int id)
        {
            return _dbContext.Purchases.Include(p => p.PurchaseDetails)
                .Where(p => p.Id == id)
                .Select(p => new PurchaseWithDetailsResponse
                {
                    Id = p.Id,
                    CustomerId = p.CustomerId,
                    RecommenderId = p.RecommenderId,
                    PurchaseDate = p.PurchaseDate,
                    PurchaseDetails = p.PurchaseDetails.Select(t => new PurchaseDetailResponse
                    {
                        Id = t.Id,
                        PriceForOnePiece = t.PriceForOnePiece,
                        ProductId = t.ProductId,
                        Quantity = t.Quantity
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<ICollection<PurchaseWithDetailsResponse>> GetPurchasesWithDetails()
        {
            return await _dbContext.Purchases.Include(p => p.PurchaseDetails)
               .Select(p => new PurchaseWithDetailsResponse
               {
                   Id = p.Id,
                   CustomerId = p.CustomerId,
                   RecommenderId = p.RecommenderId,
                   PurchaseDate = p.PurchaseDate,
                   PurchaseDetails = p.PurchaseDetails.Select(t => new PurchaseDetailResponse
                   {
                       Id = t.Id,
                       PriceForOnePiece = t.PriceForOnePiece,
                       ProductId = t.ProductId,
                       Quantity = t.Quantity
                   }).ToList()
               })
               .ToListAsync();
        }

    }
}