using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IProductService
    {
        public Task<ProductResponse> AddProduct(ProductRequest productToAdd);
        public Task<List<ProductResponse>> GetProducts();
        public Task<ProductResponse> GetProduct(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeService _productTypeService;
        public ProductService(IProductRepository productRepository, IProductTypeService productTypeService)
        {
            _productRepository = productRepository;
            _productTypeService = productTypeService;
        }

        public async Task<ProductResponse> AddProduct(ProductRequest productToAdd)
        {
            await _productTypeService.GetProductType(productToAdd.ProductTypeId);
            return await _productRepository.AddProduct(productToAdd);
        }

        public async Task<ProductResponse> GetProduct(int id)
        {
            return await _productRepository.GetProduct(id);
        }

        public async Task<List<ProductResponse>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }
    }
}