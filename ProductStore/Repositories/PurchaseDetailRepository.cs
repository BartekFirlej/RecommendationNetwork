using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IPurchaseDetailRepository
    {
        public Task<PurchaseDetail> AddPurchaseDetail(PurchaseDetailRequest purchasDetail, int orderId);
        public Task<PurchaseDetail> AddPurchaseDetail(PurchaseIdDetailRequest purchaseDetails);
    }
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly StoreDbContext _dbContext;

        public PurchaseDetailRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
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
                PurchaseId = purchaseDetail.OrderId
            };
            await _dbContext.PurchaseDetails.AddAsync(detail);
            await _dbContext.SaveChangesAsync();
            return detail;
        }
    }
}
