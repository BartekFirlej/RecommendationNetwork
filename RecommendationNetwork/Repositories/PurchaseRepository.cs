using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<PurchaseResponse> AddPurchase(PurchaseRequest purchaseToAdd);
        public Task<PurchaseResponse> AddPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd);
        public Task<List<PurchaseResponse>> GetPurchases();
        public Task<PurchaseResponse> GetPurchase(int id);
    }
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IDriver _driver;
        public PurchaseRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static PurchaseResponse MapToOrderResponse(IRecord record)
        {
            var properties = record["p"].As<INode>().Properties;

            var orderResponse = new PurchaseResponse
            {
                Id = properties["Id"].As<int>(),
                PurchaseDate = properties["PurchaseDate"].As<DateTimeOffset>()
            };

            return orderResponse;
        }

        public async Task<PurchaseResponse> AddPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addOrdedQuery = "CREATE (p:Purchase {Id: $Id, PurchaseDate: datetime($PurchaseDate)}) RETURN p";
                var addCustomerToPurchaseQuery = "MATCH (c:Customer {Id: $CustomerId}), (p:Purchase {Id: $Id}) MERGE (c)-[:PURCHASED]->(p)";
                var addPurchaseProducts = "MATCH (p:Purchase {Id: $purchaseId}), (pr:Product{Id:$productId}) MERGE (p)-[:CONISTS_OF{Id: $purchaseDetailId, Quantity: $Quantity, PriceForOnePiece: $Price}]->(pr)";
                var parameters = purchaseToAdd;

                var orderResult = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addOrdedQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                await session.WriteTransactionAsync(async transaction =>
                {
                    return await transaction.RunAsync(addCustomerToPurchaseQuery, parameters);
                });

                if(purchaseToAdd.RecommenderId != null) {
                    var addRecommenderToPurchaseQuery = "MATCH (c:Customer {Id: $RecommenderId}), (p:Purchase {Id: $Id}) MERGE (c)-[:RECOMMENDED_PURCHASE]->(p)";
                    await session.WriteTransactionAsync(async transaction =>
                    {
                        return await transaction.RunAsync(addRecommenderToPurchaseQuery, parameters);
                    });
                }

                foreach(var purchaseDetail in purchaseToAdd.PurchaseDetails)
                {
                    var purchaseId = purchaseToAdd.Id;
                    var purchaseDetailId = purchaseDetail.Id;
                    var Quantity = purchaseDetail.Quantity;
                    var productId = purchaseDetail.ProductId;
                    var Price = purchaseDetail.PriceForOnePiece;
                    await session.WriteTransactionAsync(async transaction =>
                    {
                        return await transaction.RunAsync(addPurchaseProducts, new { purchaseId, purchaseDetailId, Quantity, productId, Price });
                    });
                }

                var orderResponse = MapToOrderResponse(orderResult);
                return orderResponse;
            }
        }

        public async Task<PurchaseResponse> AddPurchase(PurchaseRequest purchaseToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addOrdedQuery = "CREATE (p:Purchase {Id: $Id, PurchaseDate: datetime($PurchaseDate)}) RETURN p";
                var addCustomerToOrderQuery = "MATCH (c:Customer {Id: $CustomerId}), (p:Purchase {Id: $Id}) MERGE (c)-[:PURCHASED]->(p)";
                var parameters = purchaseToAdd;

                var orderResult = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addOrdedQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                await session.WriteTransactionAsync(async transaction =>
                {
                    return await transaction.RunAsync(addCustomerToOrderQuery, parameters);
                });

                if (purchaseToAdd.RecommenderId != null)
                {
                    var addRecommenderToOrderQuery = "MATCH (c:Customer {Id: $RecommenderId}), (p:Purchase {Id: $Id}) MERGE (c)-[:RECOMMENDED_PURCHASE]->(p)";
                    await session.WriteTransactionAsync(async transaction =>
                    {
                        return await transaction.RunAsync(addRecommenderToOrderQuery, parameters);
                    });
                }

                var orderResponse = MapToOrderResponse(orderResult);
                return orderResponse;
            }
        }

        public async Task<PurchaseResponse> GetPurchase(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveProduct = "MATCH (p:Purchase) WHERE p.Id=$id RETURN p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveProduct, new { id });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundPurchaseException(id);
                    }
                });

                var orderResponse = MapToOrderResponse(result);

                return orderResponse;
            }
        }

        public async Task<List<PurchaseResponse>> GetPurchases()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (p:Purchase) RETURN p";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                    return await queryResult.ToListAsync();
                });

                if (!result.Any())
                    throw new NotFoundPurchaseException();

                var ordersResponse = result.Select(record => MapToOrderResponse(record)).ToList();

                return ordersResponse;
            }
        }
    }
}
