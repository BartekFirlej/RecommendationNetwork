using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

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
                var addProductTypeQuery = "CREATE (pt:ProductType {Id: $Id, Name: $Name}) RETURN pt";
                var parameters = productTypeToAdd;

                var result = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addProductTypeQuery, parameters);
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
                var retrieveProductType = "MATCH (pt:ProductType) WHERE pt.Id=$id RETURN pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveProductType, new { id });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundProductTypeException(id);
                    }
                });

                var productTypeResponse = MapToProductTypeResponse(result);

                return productTypeResponse;
            }
        }

        public async Task<List<ProductTypeResponse>> GetProductTypes()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveProductTypes = "MATCH (pt:ProductType) RETURN pt";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveProductTypes);
                    return await queryResult.ToListAsync();

                });

                if (!result.Any())
                    throw new NotFoundProductTypeException();

                var productTypesResponse = result.Select(record => MapToProductTypeResponse(record)).ToList();

                return productTypesResponse;
            }
        }
    }
}