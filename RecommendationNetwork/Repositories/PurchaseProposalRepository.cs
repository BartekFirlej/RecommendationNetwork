using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseProposalRepository
    {
        public Task<PurchaseProposalResponse> AddPurchaseProposal(PurchaseProposalRequest purchasePropsalRequest, int productId);
        public Task<List<PurchaseProposalResponse>> GetPurchaseProposals();
        public Task<PurchaseProposalResponse> GetPurchaseProposal(int customerId, int productId);
    }
    public class PurchaseProposalRepository : IPurchaseProposalRepository
    {
        private readonly IDriver _driver;
        public PurchaseProposalRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static PurchaseProposalResponse MapToPurchaseProposalResponse(IRecord record)
        {
            var propertiesCustomer = record["c"].As<INode>().Properties;
            var propertiesProposal = record["prop"].As<IRelationship>().Properties;
            var propertiesProduct = record["p"].As<INode>().Properties;

            var purchaseProposalResponse = new PurchaseProposalResponse
            {
                CustomerId = propertiesCustomer["Id"].As<int>(),
                ProductId = propertiesProduct["Id"].As<int>(),
                Date = propertiesProposal["Date"].As<DateTime>()
            };

            return purchaseProposalResponse;
        }

        public async Task<PurchaseProposalResponse> AddPurchaseProposal(PurchaseProposalRequest purchaseProposalRequest, int productId)
        {
            using (var session = _driver.AsyncSession())
            {
                var addPurchaseProposal = "MATCH (c:Customer {Id: $customerId}), (p:Product{Id:$productId}) MERGE (c)-[prop:PURCHASE_PROPOSAL{Date: $Date}]->(p) RETURN c,prop,p";
                var parameters = new { customerId = purchaseProposalRequest.CustomerId, productId = productId, Date = DateTime.Now };

                var addedPurchaseProposal = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addPurchaseProposal, parameters);
                    return await queryResult.SingleAsync();
                });

                var purchaseProposalResponse = MapToPurchaseProposalResponse(addedPurchaseProposal);
                return purchaseProposalResponse;
            }
        }

        public async Task<PurchaseProposalResponse> GetPurchaseProposal(int customerId, int productId)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrievePurchaseProposals = "MATCH (c:Customer {Id:$customerId} )-[prop:PURCHASE_PROPOSAL]->(p:Product {Id:$productId} ) RETURN c, prop, p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrievePurchaseProposals, new { customerId, productId });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundPurchaseProposalException(customerId, productId);
                    }
                });

                var purchaseProposal = MapToPurchaseProposalResponse(result);

                return purchaseProposal;
            }
        }

        public async Task<List<PurchaseProposalResponse>> GetPurchaseProposals()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrievePurchaseProposals = "MATCH (c:Customer)-[prop:PURCHASE_PROPOSAL]->(p:Product) RETURN c, prop, p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrievePurchaseProposals);
                    return await queryResult.ToListAsync();
                });

                if (!result.Any())
                    throw new NotFoundPurchaseProposalException();

                var purchaseProposals = result.Select(record => MapToPurchaseProposalResponse(record)).ToList();

                return purchaseProposals;
            }
        }
    }
}