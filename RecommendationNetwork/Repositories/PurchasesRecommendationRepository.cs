using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Models;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseRecommendationRepository
    {
        public Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations();
        public Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendation(int customerId);
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
                LastName = properties["LastName"].As<string>()
            };

            return customerResponse;
        }

        public async Task<List<PurchaseRecommendationResponse>> GetPurchasesCustomersRecommmendations()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer)" +
                    " OPTIONAL MATCH (c)-[:RECOMMENDED]->(direct:Customer)" +
                    " WITH c, direct" +
                    " OPTIONAL MATCH (direct)-[:PURCHASED]->(purchase1)" +
                    " WITH c, direct, COUNT(DISTINCT purchase1) AS purchasesByDirect" +
                    " WITH c, purchasesByDirect, indirect1" +
                    " OPTIONAL MATCH (indirect1)-[:PURCHASED]->(purchase2)" +
                    " WITH c, purchasesByDirect, indirect1, COUNT(DISTINCT purchase2) AS purchasesByIndirect1" +
                    " OPTIONAL MATCH (indirect1)-[:RECOMMENDED]->(indirect2:Customer)" +
                    " WITH c, purchasesByDirect, purchasesByIndirect1, indirect2" +
                    " OPTIONAL MATCH (indirect2)-[:PURCHASED]->(purchase3)" +
                    " WITH c, purchasesByDirect, purchasesByIndirect1, indirect2, COUNT(DISTINCT purchase3) AS purchasesByIndirect2" +
                    " RETURN c, purchasesByDirect, purchasesByIndirect1, purchasesByIndirect2,";

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

        public async Task<PurchaseRecommendationResponse> GetPurchasesCustomerRecommmendation(int customerId)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer {Id: $customerId})" +
                    " OPTIONAL MATCH (c)-[:RECOMMENDED]->(direct:Customer)" +
                    " WITH c, direct" +
                    " OPTIONAL MATCH (direct)-[:PURCHASED]->(purchase1)" +
                    " WITH c, direct, COUNT(DISTINCT purchase1) AS purchasesByDirect" +
                    " WITH c, purchasesByDirect, indirect1" +
                    " OPTIONAL MATCH (indirect1)-[:PURCHASED]->(purchase2)" +
                    " WITH c, purchasesByDirect, indirect1, COUNT(DISTINCT purchase2) AS purchasesByIndirect1" +
                    " OPTIONAL MATCH (indirect1)-[:RECOMMENDED]->(indirect2:Customer)" +
                    " WITH c, purchasesByDirect, purchasesByIndirect1, indirect2" +
                    " OPTIONAL MATCH (indirect2)-[:PURCHASED]->(purchase3)" +
                    " WITH c, purchasesByDirect, purchasesByIndirect1, indirect2, COUNT(DISTINCT purchase3) AS purchasesByIndirect2" +
                    " RETURN c, purchasesByDirect, purchasesByIndirect1, purchasesByIndirect2,";

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
