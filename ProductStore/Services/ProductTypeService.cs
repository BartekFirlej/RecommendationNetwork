using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IProductTypeService {
        public Task<ICollection<ProductTypeResponse>> GetProductTypes();
        public Task<ProductTypeResponse> GetProductTypeResponse(int id);
        public Task<ProductType> GetProductType(int id);
        public Task<ProductType> DeleteProductType(int id);
        public Task<ProductType> PostProductType(ProductTypeRequest productTypeToAdd);
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeService(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public async Task<ICollection<ProductTypeResponse>> GetProductTypes()
        {
            var productTypes = await _productTypeRepository.GetProductTypes();
            if (!productTypes.Any())
                throw new Exception("Not found any product type.");
            return productTypes;
        }

        public async Task<ProductTypeResponse> GetProductTypeResponse(int id)
        {
            var productType = await _productTypeRepository.GetProductTypeResponse(id);
            if (productType == null)
                throw new Exception(String.Format("Not found product type with id {0}.", id));
            return productType;
        }

        public async Task<ProductType> GetProductType(int id)
        {
            var productType = await _productTypeRepository.GetProductType(id);
            if (productType == null)
                throw new Exception(String.Format("Not found product type with id {0}.", id));
            return productType;
        }

        public async Task<ProductType> DeleteProductType(int id)
        {
            var productType = await GetProductType(id);
            var deletedProductType = await _productTypeRepository.DeleteProductType(productType);
            return deletedProductType;
        }

        public async Task<ProductType> PostProductType(ProductTypeRequest productTypeToAdd)
        {
            if (char.IsLower(productTypeToAdd.Name[0]))
                throw new Exception("Napisales z malej litery.");
            var addedProductType = await _productTypeRepository.PostProductType(productTypeToAdd);
            return addedProductType;
        }

        
    }
}
