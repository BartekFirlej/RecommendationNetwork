using Neo4j.Driver;
using RecommendationNetwork.DTOs;

namespace RecommendationNetwork.Repositories
{
    public interface IProductTypeRepository
    {
        public Task<ProductTypeResponse> AddProductType(ProductTypeRequest productTypeToAdd);
        public Task<List<ProductTypeResponse>> GetProductTypes();
        public Task<ProductTypeResponse> GetProductType(int id);
    }
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly IDriver _driver;
        public ProductTypeRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static ProductTypeResponse MapToProductTypeResponse(IRecord record)
        {
            var properties = record["pt"].As<INode>().Properties;

            var productTypeResponse = new ProductTypeResponse
            {
                Id = properties["Id"].As<int>(),
                Name = properties["Name"].As<string>()
            };

            return productTypeResponse;
        }

        public async Task<ProductTypeResponse> AddProductType(ProductTypeRequest productTypeToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addVoivodeshipQuery = "CREATE (pt:ProductType {Id: $Id, Name: $Name}) RETURN pt";
                var parameters = productTypeToAdd;

                var result = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addVoivodeshipQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                var productTypeResponse = MapToProductTypeResponse(result);

                return productTypeResponse;
            }
        }

        public async Task<ProductTypeResponse> GetProductType(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (pt:ProductType) WHERE pt.Id=$id RETURN pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher, new { id });
                    return await queryResult.SingleAsync();
                });

                var productTypeResponse = MapToProductTypeResponse(result);

                return productTypeResponse;
            }
        }

        public async Task<List<ProductTypeResponse>> GetProductTypes()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (pt:ProductType) RETURN pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();
                });

                var voivodeshipResponse = result.Select(record => MapToProductTypeResponse(record)).ToList();

                return voivodeshipResponse;
            }
        }
    }
}
