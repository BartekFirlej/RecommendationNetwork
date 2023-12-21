using ProductStore.Models;
using ProductStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProductStore.Repositories
{
    public interface IProductTypeRepository
    {
        public Task<ICollection<ProductTypeResponse>> GetProductTypes();
        public Task<ProductTypeResponse> GetProductTypeResponse(int id);
        public Task<ProductType> GetProductType(int id);
        public Task<ProductType> DeleteProductType(ProductType productTypeToDelete);
        public Task<ProductType> PostProductType(ProductTypeRequest productTypeToAdd);
    }
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly StoreDbContext _dbContext;

        public ProductTypeRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<ProductTypeResponse>> GetProductTypes()
        {
            var productTypes = await _dbContext.ProductTypes.Select(p => new ProductTypeResponse
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();
            return productTypes;
        }

        public async Task<ProductTypeResponse> GetProductTypeResponse(int id)
        {
            var productType = await _dbContext.ProductTypes.Select(p => new ProductTypeResponse
            {
                Id = p.Id,
                Name = p.Name
            }).Where(p => p.Id == id).FirstOrDefaultAsync();
            return productType;
        }

        public async Task<ProductType> GetProductType(int id)
        {
            var productType = await _dbContext.ProductTypes.Where(p => p.Id == id).FirstOrDefaultAsync();
            return productType;
        }

        public async Task<ProductType> DeleteProductType(ProductType productTypeToDelete)
        {
            _dbContext.ProductTypes.Remove(productTypeToDelete);  
            await _dbContext.SaveChangesAsync();
            return productTypeToDelete;
        }

        public async Task<ProductType> PostProductType(ProductTypeRequest productTypeToAdd)
        {
            var newProductType = new ProductType 
            {
                Name = productTypeToAdd.Name
            };
            await _dbContext.ProductTypes.AddAsync(newProductType);
            await _dbContext.SaveChangesAsync();
            return newProductType;
        }

        
    }
}
