using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IPurchaseDetailRepository
    {
        public Task<ICollection<PurchaseDetailResponse>> GetPurchaseDetails();
        public Task<ICollection<PurchaseDetail>> GetPurchaseDetails(int orderId);
        public Task<PurchaseDetailResponse> GetPurchaseDetailResponse(int id);
        public Task<PurchaseDetail> GetPurchaseDetail(int id);
        public Task<PurchaseDetail> AddPurchaseDetail(PurchaseDetailRequest purchasDetail, int orderId);
        public Task<PurchaseDetail> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetails);
        public Task<PurchaseDetail> DeletePurchaseDetail(PurchaseDetail purchaseDetailToDelete);
        public Task<ICollection<PurchaseDetail>> DeletePurchaseDetail(ICollection<PurchaseDetail> purchaseDetailToDelete);
    }
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly StoreDbContext _dbContext;

        public PurchaseDetailRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<PurchaseDetailResponse>> GetPurchaseDetails()
        {
            return await _dbContext.PurchaseDetails
                .Select(p => new PurchaseDetailResponse
                {
                    Id = p.Id,
                    PurchaseId = p.PurchaseId,
                    PriceForOnePiece = p.PriceForOnePiece,
                    Quantity = p.Number,
                    ProductId = p.ProductId
                }).ToListAsync();
        }

        public async Task<PurchaseDetail> GetPurchaseDetail(int id)
        {
            return await _dbContext.PurchaseDetails
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseDetailResponse> GetPurchaseDetailResponse(int id)
        {
            return await _dbContext.PurchaseDetails
                .Select(p => new PurchaseDetailResponse
                {
                    Id = p.Id,
                    PurchaseId = p.PurchaseId,
                    PriceForOnePiece = p.PriceForOnePiece,
                    Quantity = p.Number,
                    ProductId = p.ProductId
                })
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PurchaseDetail> AddPurchaseDetail(PurchaseDetailRequest purchaseDetail, int orderId)
        {
            var detail = new PurchaseDetail
            {
                Number = purchaseDetail.Quantity,
                PriceForOnePiece = purchaseDetail.PriceForOnePiece,
                ProductId = purchaseDetail.ProductId,
                PurchaseId = orderId
            };
            await _dbContext.PurchaseDetails.AddAsync(detail);
            await _dbContext.SaveChangesAsync();
            return detail;
        }

        public async Task<PurchaseDetail> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetail)
        {
            var detail = new PurchaseDetail
            {
                Number = purchaseDetail.Quantity,
                PriceForOnePiece = purchaseDetail.PriceForOnePiece,
                ProductId = purchaseDetail.ProductId,
                PurchaseId = purchaseDetail.PurchaseId
            };
            await _dbContext.PurchaseDetails.AddAsync(detail);
            await _dbContext.SaveChangesAsync();
            return detail;
        }

        public async Task<PurchaseDetail> DeletePurchaseDetail(PurchaseDetail purchaseDetailToDelete)
        {
            _dbContext.PurchaseDetails.Remove(purchaseDetailToDelete);
            await _dbContext.SaveChangesAsync();
            return purchaseDetailToDelete;
        }

        public async Task<ICollection<PurchaseDetail>> GetPurchaseDetails(int orderId)
        {
            return await _dbContext.PurchaseDetails
                .Where(p => p.PurchaseId == orderId)
                .ToListAsync();
        }

        public async Task<ICollection<PurchaseDetail>> DeletePurchaseDetail(ICollection<PurchaseDetail> purchaseDetailToDelete)
        {
            _dbContext.PurchaseDetails.RemoveRange(purchaseDetailToDelete);
            await _dbContext.SaveChangesAsync();
            return purchaseDetailToDelete;
        }
    }
}