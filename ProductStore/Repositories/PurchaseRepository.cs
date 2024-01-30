using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<ICollection<PurchaseResponse>> GetPurchases();
        public Task<PagedList<PurchaseResponse>> GetPurchasesPaged(int page, int size);
        public Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId);
        public Task<PagedList<PurchaseResponse>> GetCustomersPurchasesPaged(int customerId, int page, int size);
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
                .Include(c => c.Purchase.Customer)
                .Include(c => c.Purchase.Recommender)
                .GroupBy(p => p.Purchase.Id)
                .Select(group => new PurchaseResponse
                {
                    Id = group.Key,
                    PurchaseDate = group.First().Purchase.PurchaseDate,
                    CustomerId = group.First().Purchase.CustomerId,
                    CustomerName = group.First().Purchase.Customer.Name,
                    CustomerLastName = group.First().Purchase.Customer.LastName,
                    RecommenderId = group.First().Purchase.RecommenderId,
                    RecommenderName = group.First().Purchase.Recommender.Name,
                    RecommenderLastName = group.First().Purchase.Recommender.LastName,
                    Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity)
                }).ToListAsync();
        }

        public async Task<ICollection<PurchaseResponse>> GetCustomersPurchases(int customerId)
        {
            return await _dbContext.PurchaseDetails
                .Include(p => p.Purchase)
                .Include(c => c.Purchase.Customer)
                .Include(c => c.Purchase.Recommender)
                .GroupBy(p => p.Purchase.Id)
                .Select(group => new PurchaseResponse
                {
                    Id = group.Key,
                    PurchaseDate = group.First().Purchase.PurchaseDate,
                    CustomerId = group.First().Purchase.CustomerId,
                    CustomerName = group.First().Purchase.Customer.Name,
                    CustomerLastName = group.First().Purchase.Customer.LastName,
                    RecommenderId = group.First().Purchase.RecommenderId,
                    RecommenderName = group.First().Purchase.Recommender.Name,
                    RecommenderLastName = group.First().Purchase.Recommender.LastName,
                    Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity)
                })
               .Where(p => p.CustomerId == customerId)
               .ToListAsync();
        }

        public async Task<PagedList<PurchaseResponse>> GetPurchasesPaged(int page, int size)
        {
            return await PagedList<PurchaseResponse>.Create(
                _dbContext.PurchaseDetails
                .Include(p => p.Purchase)
                .Include(c => c.Purchase.Customer)
                .Include(c => c.Purchase.Recommender)
                .GroupBy(p => p.Purchase.Id)
                .Select(group => new PurchaseResponse
                {
                    Id = group.Key,
                    PurchaseDate = group.First().Purchase.PurchaseDate,
                    CustomerId = group.First().Purchase.CustomerId,
                    CustomerName = group.First().Purchase.Customer.Name,
                    CustomerLastName = group.First().Purchase.Customer.LastName,
                    RecommenderId = group.First().Purchase.RecommenderId,
                    RecommenderName = group.First().Purchase.Recommender.Name,
                    RecommenderLastName = group.First().Purchase.Recommender.LastName,
                    Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity)
                })
                .OrderBy(p => p.Id),
                page,
                size);
        }

        public async Task<PagedList<PurchaseResponse>> GetCustomersPurchasesPaged(int customerId, int page, int size)
        {
            return await PagedList<PurchaseResponse>.Create(
                _dbContext.PurchaseDetails
                .Include(p => p.Purchase)
                .Include(c => c.Purchase.Customer)
                .Include(c => c.Purchase.Recommender)
                .GroupBy(p => p.Purchase.Id)
                .Select(group => new PurchaseResponse
                {
                    Id = group.Key,
                    PurchaseDate = group.First().Purchase.PurchaseDate,
                    CustomerId = group.First().Purchase.CustomerId,
                    CustomerName = group.First().Purchase.Customer.Name,
                    CustomerLastName = group.First().Purchase.Customer.LastName,
                    RecommenderId = group.First().Purchase.RecommenderId,
                    RecommenderName = group.First().Purchase.Recommender.Name,
                    RecommenderLastName = group.First().Purchase.Recommender.LastName,
                    Amount = group.Sum(item => item.PriceForOnePiece * item.Quantity)
                })
               .Where(p => p.CustomerId == customerId)
               .OrderBy(p => p.Id),
                page,
                size);
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
               .Include(c => c.Purchase.Customer)
               .Include(c => c.Purchase.Recommender)
               .GroupBy(p => p.Purchase.Id)
               .Select(group => new PurchaseResponse
               {
                   Id = group.Key,
                   PurchaseDate = group.First().Purchase.PurchaseDate,
                   CustomerId = group.First().Purchase.CustomerId,
                   CustomerName = group.First().Purchase.Customer.Name,
                   CustomerLastName = group.First().Purchase.Customer.LastName,
                   RecommenderId = group.First().Purchase.RecommenderId,
                   RecommenderName = group.First().Purchase.Recommender.Name,
                   RecommenderLastName = group.First().Purchase.Recommender.LastName,
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

        public async Task<PurchaseWithDetailsResponse> GetPurchaseWithDetails(int id)
        {
            return await _dbContext.PurchaseDetails
        .Where(p => p.Purchase.Id == id)
        .Include(p => p.Purchase)
        .Include(p => p.Purchase.Customer)
        .Include(p => p.Purchase.Recommender)
        .Include(p => p.Product)
        .Include(p => p.Product.ProductType)
        .Select(p => new
        {
            Purchase = p.Purchase,
            PurchaseDetail = p
        })
        .ToListAsync()
        .ContinueWith(task =>
        {
            var results = task.Result;
            if (!results.Any()) return null;

            var firstResult = results.First();
            var purchase = firstResult.Purchase;

            return new PurchaseWithDetailsResponse
            {
                Id = purchase.Id,
                CustomerId = purchase.CustomerId,
                CustomerName = purchase.Customer.Name,
                CustomerLastName = purchase.Customer.LastName,
                RecommenderId = purchase.RecommenderId,
                RecommenderName = purchase.Recommender?.Name,
                RecommenderLastName = purchase.Recommender?.LastName,
                PurchaseDate = purchase.PurchaseDate,
                PurchaseDetails = results.Select(r => new PurchaseDetailResponse
                {
                    Id = r.PurchaseDetail.Id,
                    PriceForOnePiece = r.PurchaseDetail.PriceForOnePiece,
                    ProductId = r.PurchaseDetail.ProductId,
                    ProductName = r.PurchaseDetail.Product.Name,
                    ProductTypeId = r.PurchaseDetail.Product.ProductTypeId,
                    ProductTypeName = r.PurchaseDetail.Product.ProductType.Name,
                    Quantity = r.PurchaseDetail.Quantity
                }).ToList()
            };
        });
        }

        public async Task<ICollection<PurchaseWithDetailsResponse>> GetPurchasesWithDetails()
        {
            return await _dbContext.Purchases
                .Include(p => p.Customer)
                .Include(p => p.Recommender)
                .Include(p => p.PurchaseDetails)
                    .ThenInclude(pd => pd.Product)
                        .ThenInclude(pr => pr.ProductType)
                .Select(p => new PurchaseWithDetailsResponse
                {
                    Id = p.Id,
                    CustomerId = p.CustomerId,
                    CustomerName = p.Customer.Name,
                    CustomerLastName = p.Customer.LastName,
                    RecommenderId = p.Recommender != null ? p.Recommender.Id : (int?)null, // Handle nullable Recommender
                    RecommenderName = p.Recommender != null ? p.Recommender.Name : null, // Handle nullable Recommender
                    RecommenderLastName = p.Recommender != null ? p.Recommender.LastName : null, // Handle nullable Recommender
                    PurchaseDate = p.PurchaseDate,
                    PurchaseDetails = p.PurchaseDetails.Select(pd => new PurchaseDetailResponse
                    {
                        Id = pd.Id,
                        PriceForOnePiece = pd.PriceForOnePiece,
                        ProductId = pd.ProductId,
                        ProductName = pd.Product.Name,
                        ProductTypeId = pd.Product.ProductTypeId,
                        ProductTypeName = pd.Product.ProductType.Name,
                        Quantity = pd.Quantity
                    }).ToList()
                }).ToListAsync();
        }

    }
}