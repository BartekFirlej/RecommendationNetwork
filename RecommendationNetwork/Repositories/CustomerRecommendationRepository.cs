using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface ICustomerRecommendationRepository
    {
        public Task<List<CustomerRecommendationResponse>> GetCustomersCustomersRecommmendations();
        public Task<CustomerRecommendationResponse> GetCustomersCustomerRecommmendation(int customerId);
    }
    public class CustomerRecommendationRepository : ICustomerRecommendationRepository
    {
        private readonly IDriver _driver;

        public CustomerRecommendationRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static CustomerRecommendationResponse MapToCustomerRecommendationResponse(IRecord record)
        {
            var properties = record["c"].As<INode>().Properties;

            var customerRecommendationResponse = new CustomerRecommendationResponse
            {
                Id = properties["Id"].As<int>(),
                Name = properties["Name"].As<string>(),
                LastName = properties["LastName"].As<string>(),
                FirstLevelRecommendations = record["directRecommendations"].As<int>(), 
                SecondLevelRecommendations = record["indirect1Recommendations"].As<int>(), 
                ThirdLevelRecommendations = record["indirect2Recommendations"].As<int>() 
            };

            return customerRecommendationResponse;
        }


        public async Task<List<CustomerRecommendationResponse>> GetCustomersCustomersRecommmendations()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer)\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->(direct:Customer)\r\nWITH c, COUNT(DISTINCT direct) AS directRecommendations\r\n\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->(indirect1:Customer)\r\nWITH c, directRecommendations, COUNT(DISTINCT indirect1) AS indirect1Recommendations\r\n\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->(indirect2:Customer)\r\nRETURN c, directRecommendations, indirect1Recommendations, COUNT(DISTINCT indirect2) AS indirect2Recommendations";

                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();
                });

                if (!result.Any())
                    throw new NotFoundCustomerRecommendationException();

                var customerRecommendations = result.Select(record => MapToCustomerRecommendationResponse(record)).ToList();

                return customerRecommendations;
            }
        }

        public async Task<CustomerRecommendationResponse> GetCustomersCustomerRecommmendation(int customerId)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer {Id:$customerId})\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->(direct:Customer)\r\nWITH c, COUNT(DISTINCT direct) AS directRecommendations\r\n\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->(indirect1:Customer)\r\nWITH c, directRecommendations, COUNT(DISTINCT indirect1) AS indirect1Recommendations\r\n\r\nOPTIONAL MATCH (c)-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->()-[:RECOMMENDED_CUSTOMER]->(indirect2:Customer)\r\nRETURN c, directRecommendations, indirect1Recommendations, COUNT(DISTINCT indirect2) AS indirect2Recommendations";

                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveNodesCypher, new { customerId });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundCustomerRecommendationException(customerId);
                    }
                });

                var customerRecommendations = MapToCustomerRecommendationResponse(result);

                return customerRecommendations;
            }
        }

        
    }
}
