using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Models;

namespace ProductStore.Repositories
{
    public interface IProductRepository
    {
        public Task<ICollection<ProductResponse>> GetProducts();
        public Task<ICollection<ProductResponse>> GetProductsByIds(IdsListDTO ids);
        public Task<ProductResponse> GetProductResponse(int id);
        public Task<Product> GetProduct(int id);
        public Task<Product> DeleteProduct(Product productToDelete);
        public Task<Product> PostProduct(ProductRequest productToAdd);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly StoreDbContext _dbContext;
        public ProductRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<ProductResponse>> GetProducts()
        {
            var products = await _dbContext.Products
                   .Include(p => p.ProductType)
                   .Select(p => new ProductResponse
                   {
                       Id = p.Id,
                       Name = p.Name,
                       Price = p.Price,
                       ProductTypeId = p.ProductTypeId,
                       ProductTypeName = p.ProductType.Name
                   }).ToListAsync();
            return products;
        }


        public async Task<ICollection<ProductResponse>> GetProductsByIds(IdsListDTO ids)
        {
            var products = await _dbContext.Products
                   .Include(p => p.ProductType)
                   .Where(p => ids.ids.Contains(p.Id))
                   .Select(p => new ProductResponse
                   {
                       Id = p.Id,
                       Name = p.Name,
                       Price = p.Price,
                       ProductTypeId = p.ProductTypeId,
                       ProductTypeName = p.ProductType.Name
                   }).ToListAsync();
            return products;
        }

        public async Task<ProductResponse> GetProductResponse(int id)
        {
            var product = await _dbContext.Products
                   .Include(p => p.ProductType)
                   .Select(p => new ProductResponse
                   {
                       Id = p.Id,
                       Name = p.Name,
                       Price = p.Price,
                       ProductTypeId = p.ProductTypeId,
                       ProductTypeName = p.ProductType.Name
                   })
                   .Where(p => p.Id == id)
                   .FirstOrDefaultAsync();
            return product;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(Product productToDelete)
        {
            _dbContext.Products.Remove(productToDelete);
            await _dbContext.SaveChangesAsync();
            return productToDelete;
        }

        public async Task<Product> PostProduct(ProductRequest productToAdd)
        {
            var newProduct = new Product
            {
                Price = productToAdd.Price,
                ProductTypeId = productToAdd.ProductTypeId,
                Name = productToAdd.Name
            };
            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            return newProduct;
        }
    }
}