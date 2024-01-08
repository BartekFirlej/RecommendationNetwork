using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseRecommendationRepository
    {
        public Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations();
        public Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendations(int customerId);
    }
    public class PurchaseRecommendationRepository : IPurchaseRecommendationRepository
    {
        public readonly IDriver _driver;

        public PurchaseRecommendationRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static PurchaseRecommendationResponse MapToPurchaseRecommendationResponse(IRecord record)
        {
            var properties = record["c"].As<INode>().Properties;

            var customerResponse = new PurchaseRecommendationResponse
            {
                Id = properties["Id"].As<int>(),
                Name = properties["Name"].As<string>(),
                LastName = properties["LastName"].As<string>(),
                FirstLevelRecommendations = record["directRecommendedPurchases"].As<int>(),
                SecondLevelRecommendations = record["indirect1RecommendedPurchases"].As<int>(),
                ThirdLevelRecommendations = record["indirect2RecommendedPurchases"].As<int>()
            };

            return customerResponse;
        }

        public async Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer)\r\n// First level of recommendation purchases\r\nOPTIONAL MATCH (c)-[direct:RECOMMENDED_PURCHASE]->()\r\nWITH c, COUNT(direct) AS directRecommendedPurchases\r\n\r\n// Second level of recommendation purchases\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[indirect1:RECOMMENDED_PURCHASE]->()\r\nWITH c, directRecommendedPurchases, COUNT(indirect1) AS indirect1RecommendedPurchases\r\n\r\n// Third level of recommendation purchases\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->()-[indirect2:RECOMMENDED_PURCHASE]->()\r\nRETURN c, directRecommendedPurchases, indirect1RecommendedPurchases, COUNT(indirect2) AS indirect2RecommendedPurchases\r\n";

                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();
                });

                if (!result.Any())
                    throw new NotFoundPurchasesRecommendationsException();

                var purchasesRecommendations = result.Select(record => MapToPurchaseRecommendationResponse(record)).ToList();

                return purchasesRecommendations;
            }
        }

        public async Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendations(int customerId)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer {Id:$customerId})\r\n// First level of recommendation purchases\r\nOPTIONAL MATCH (c)-[direct:RECOMMENDED_PURCHASE]->()\r\nWITH c, COUNT(direct) AS directRecommendedPurchases\r\n\r\n// Second level of recommendation purchases\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[indirect1:RECOMMENDED_PURCHASE]->()\r\nWITH c, directRecommendedPurchases, COUNT(indirect1) AS indirect1RecommendedPurchases\r\n\r\n// Third level of recommendation purchases\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->()-[indirect2:RECOMMENDED_PURCHASE]->()\r\nRETURN c, directRecommendedPurchases, indirect1RecommendedPurchases, COUNT(indirect2) AS indirect2RecommendedPurchases\r\n";

                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveNodesCypher, new { customerId });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundPurchasesRecommendationsException(customerId);
                    }
                });

                var purchasesRecommendations = MapToPurchaseRecommendationResponse(result);

                return purchasesRecommendations;
            }
        }

    }
}
