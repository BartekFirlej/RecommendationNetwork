using AutoMapper;
using ProductStore.DTOs;
using ProductStore.Models;
using ProductStore.Repositories;

namespace ProductStore.Services
{
    public interface IProductService
    {
        public Task<ICollection<ProductResponse>> GetProducts();
        public Task<ProductResponse> GetProductResponse(int id);
        public Task<Product> GetProduct(int id);
        public Task<Product> DeleteProduct(int id);
        public Task<ProductPostResponse> PostProduct(ProductRequest productToAdd);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeService _productTypeService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IProductTypeService productTypeService, IMapper mapper)
        {
            _productRepository = productRepository;
            _productTypeService = productTypeService;
            _mapper = mapper;
        }

        public async Task<ICollection<ProductResponse>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            if (!products.Any())
                throw new Exception("Not found any products.");
            return products;
        }

        public async Task<ProductResponse> GetProductResponse(int id)
        {
            var product = await _productRepository.GetProductResponse(id);
            if (product == null)
                throw new Exception(String.Format("Not found product with id {0}.", id));
            return product;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
                throw new Exception(String.Format("Not found product with id {0}.", id));
            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var productToDelete = await GetProduct(id);
            var deletedProduct = await _productRepository.DeleteProduct(productToDelete);
            return deletedProduct;
        }

        public async Task<ProductPostResponse> PostProduct(ProductRequest productToAdd)
        {
            await _productTypeService.GetProductType(productToAdd.ProductTypeId);
            var addedProduct = await _productRepository.PostProduct(productToAdd);
            return _mapper.Map<ProductPostResponse>(addedProduct);
        }
    }
}
