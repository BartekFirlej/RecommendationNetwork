using Neo4j.Driver;
using RecommendationNetwork.DTOs;

namespace RecommendationNetwork.Repositories
{
    public interface ICustomerRepository
    {
        public Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd);
        public Task<List<CustomerResponse>> GetCustomers();
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDriver _driver;
        public CustomerRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<CustomerResponse> AddCustomer(CustomerRequest customerToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addCustomerQuery = "CREATE (c:Customer {PESEL: $PESEL, Name: $Name, SecondName: $SecondName}) RETURN c";
                var addCustomerToVoivodeshipQuery = "MATCH (c:Customer {PESEL: $PESEL}), (v:Voivodeship {Id: $VoivodeshipId}) MERGE (c)-[:LIVES_IN]->(v) RETURN c, v";
                var parameters = customerToAdd;

                var result = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addCustomerQuery, parameters);
                    await transaction.RunAsync(addCustomerToVoivodeshipQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                var customerNode = result["c"].As<INode>().Properties;
                var customerResponse = new CustomerResponse
                {
                    PESEL = customerNode["PESEL"].As<string>(),
                    Name = customerNode["Name"].As<string>(),
                    SecondName = customerNode["SecondName"].As<string>()
                };
                return customerResponse;
            }
        }

        public CustomerResponse MapToCustomerResponse(IRecord record)
        {
            var properties = record["c"].As<INode>().Properties;

            var customerResponse = new CustomerResponse
            {
                PESEL = properties["PESEL"].As<string>(),
                Name = properties["Name"].As<string>(),
                SecondName = properties["SecondName"].As<string>()
            };

            return customerResponse;
        }

        public async Task<List<CustomerResponse>> GetCustomers()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (c:Customer) RETURN c";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();
                });

                var customerResponses = result.Select(record => MapToCustomerResponse(record)).ToList();

                return customerResponses;
            }
        }
    }
}
