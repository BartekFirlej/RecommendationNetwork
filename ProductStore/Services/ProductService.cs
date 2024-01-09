using AutoMapper;
using Newtonsoft.Json.Linq;
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
        public Task<ProductPostResponse> DeleteProduct(int id);
        public Task<ProductPostResponse> PostProduct(ProductRequest productToAdd);
        public Task<ProductPostResponse> PostProductFromAPI();
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeService _productTypeService;
        private readonly IMapper _mapper;
        private readonly RabbitMqPublisher _rabbitMqPublisher;

        public ProductService(IProductRepository productRepository, IProductTypeService productTypeService, IMapper mapper, RabbitMqPublisher rabbitMqPublisher)
        {
            _productRepository = productRepository;
            _productTypeService = productTypeService;
            _mapper = mapper;
            _rabbitMqPublisher = rabbitMqPublisher;
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

        public async Task<ProductPostResponse> DeleteProduct(int id)
        {
            var productToDelete = await GetProduct(id);
            var deletedProduct = await _productRepository.DeleteProduct(productToDelete);
            return _mapper.Map<ProductPostResponse>(deletedProduct);
        }

        public async Task<ProductPostResponse> PostProduct(ProductRequest productToAdd)
        {
            if (productToAdd.Price <= 0)
                throw new Exception("Price must be greater than 0.");
            await _productTypeService.GetProductType(productToAdd.ProductTypeId);
            var addedProduct = await _productRepository.PostProduct(productToAdd);
            var addedProductResponse =  _mapper.Map<ProductPostResponse>(addedProduct);
            _rabbitMqPublisher.PublishMessage(addedProductResponse, "productQueue");
            return addedProductResponse;
        }

        public async Task<ProductPostResponse> PostProductFromAPI()
        {
            Product addedProduct = new Product();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Random random = new Random();
                    int productId = random.Next(1, 21);
                    string url = $"https://fakestoreapi.com/products/{productId}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(jsonResponse);

                        JObject parsedJson = JObject.Parse(jsonResponse);
                        string category = parsedJson["category"].ToString();
                        string name = parsedJson["title"].ToString();
                        if (name.Length > 29)
                        {
                            name = name.Substring(0, 29);
                        }
                        float price = parsedJson["price"].Value<float>();

                        ProductTypeResponse productTypeResponse;
                        try
                        {
                            productTypeResponse = await _productTypeService.GetProductTypeResponse(category);
                        }
                        catch (Exception e)
                        {
                            var ProductTypeToAdd = new ProductTypeRequest { Name = category };
                            productTypeResponse = await _productTypeService.PostProductType(ProductTypeToAdd);
                        }

                        var productToAdd = new ProductRequest { ProductTypeId = productTypeResponse.Id, Price = price, Name = name };
                        addedProduct = await _productRepository.PostProduct(productToAdd);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: " + e.Message);
                }
            }
            var addedProductResponse = _mapper.Map<ProductPostResponse>(addedProduct);
            _rabbitMqPublisher.PublishMessage(addedProductResponse, "productQueue");
            return addedProductResponse;
        }
    }
}