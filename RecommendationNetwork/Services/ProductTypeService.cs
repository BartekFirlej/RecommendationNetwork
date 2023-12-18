using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IProductTypeService
    {
        public Task<ProductTypeResponse> AddProductType(ProductTypeRequest productTypeToAdd);
        public Task<List<ProductTypeResponse>> GetProductTypes();
        public Task<ProductTypeResponse> GetProductType(int id);
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypeService(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public async Task<ProductTypeResponse> AddProductType(ProductTypeRequest productTypeToAdd)
        {
            return await _productTypeRepository.AddProductType(productTypeToAdd);
        }

        public async Task<ProductTypeResponse> GetProductType(int id)
        {
            return await _productTypeRepository.GetProductType(id);
        }

        public async Task<List<ProductTypeResponse>> GetProductTypes()
        {
            return await _productTypeRepository.GetProductTypes();
        }
    }
}
