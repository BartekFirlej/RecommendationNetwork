using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IPurchaseRepository
    {
        public Task<PurchaseResponse> AddPurchase(PurchaseRequest purchaseToAdd);
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

        /*public async Task<PurchaseResponse> AddOrder(OrderRequest orderToAdd)
        {
            using (var session = _driver.AsyncSession())
            {
                var addOrdedQuery = "CREATE (o:Order {Id: $Id, OrderDate: datetime($OrderDate)}) RETURN o";
                var addCustomerToOrderQuery = "MATCH (c:Customer {Id: $CustomerId}), (o:Order {Id: $Id}) MERGE (c)-[:PLACED_ORDER]->(o)";
                var addRecommenderToOrderQuery = "MATCH (c:Customer {Id: $RecommenderId}), (o:Order {Id: $Id}) MERGE (c)-[:RECOMMENDED_ORDER]->(o)";
                var addOrderProducts = "MATCH (o:Order {Id: $orderId}), (p:Product{Id:$Id}) MERGE (o)-[:CONISTS_OF{Quantity: $Quantity}]->(p)";
                var parameters = orderToAdd;

                var orderResult = await session.WriteTransactionAsync(async transaction =>
                {
                    var queryResult = await transaction.RunAsync(addOrdedQuery, parameters);
                    return await queryResult.SingleAsync();
                });

                await session.WriteTransactionAsync(async transaction =>
                {
                    return await transaction.RunAsync(addCustomerToOrderQuery, parameters);
                });

                foreach(var orderDetail in orderToAdd.OrderDetails)
                {
                    var Id = orderDetail.Id;
                    var Quantity = orderDetail.Quantity;
                    var orderId = orderToAdd.Id;
                    await session.WriteTransactionAsync(async transaction =>
                    {
                        return await transaction.RunAsync(addOrderProducts, new { orderId , Id, Quantity });
                    });
                }

                if (orderToAdd.RecommenderId != null)
                {
                    await session.WriteTransactionAsync(async transaction =>
                    {
                        return await transaction.RunAsync(addRecommenderToOrderQuery, parameters);
                    });
                }

                var orderResponse = MapToOrderResponse(orderResult);
                return orderResponse;
            }
        }
        */

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
