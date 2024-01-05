using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IProductRepository
    {
        public Task<ProductResponse> AddProduct(ProductRequest productToAdd);
        public Task<List<ProductResponse>> GetProducts();
        public Task<ProductResponse> GetProduct(int id);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly IDriver _driver;
        public ProductRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static ProductResponse MapToProductResponse(IRecord record)
        {
            var properties = record["p"].As<INode>().Properties;

            var productResponse = new ProductResponse
            {
                Id = properties["Id"].As<int>(),
                Name = properties["Name"].As<string>()
            };

            return productResponse;
        }

        public async Task<ProductResponse> AddProduct(ProductRequest productToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addProductQuery = "CREATE (p:Product {Id: $Id, Name: $Name}) RETURN p";
                var addProductToCategoryQuery = "MATCH (p:Product {Id: $Id}), (pt:ProductType {Id: $ProductTypeId}) MERGE (p)-[:IS_TYPE]->(pt)";

                var parameters = productToAdd;

                var result = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addProductQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                await session.WriteTransactionAsync(async transaction =>
                {
                    return await transaction.RunAsync(addProductToCategoryQuery, parameters);
                });

                var productResponse = MapToProductResponse(result);

                return productResponse;
            }
        }

        public async Task<ProductResponse> GetProduct(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveProduct = "MATCH (p:Product) WHERE p.Id=$id RETURN p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveProduct, new { id });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundProductException(id);
                    }
                });

                var productResponse = MapToProductResponse(result);

                return productResponse;
            }
        }

        public async Task<List<ProductResponse>> GetProducts()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (p:Product) RETURN p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();

                });

                if (!result.Any())
                    throw new NotFoundProductException();

                var productsResponse = result.Select(record => MapToProductResponse(record)).ToList();

                return productsResponse;
            }
        }
    }
}
