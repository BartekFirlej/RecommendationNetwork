using Neo4j.Driver;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;

namespace RecommendationNetwork.Repositories
{
    public interface IOrderRepository
    {
        public Task<OrderResponse> AddOrderWithDetails(OrderRequest orderToAdd);
        public Task<List<OrderResponse>> GetOrders();
        public Task<OrderResponse> GetOrder(int id);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly IDriver _driver;
        public OrderRepository(IDriver driver)
        {
            _driver = driver;
        }

        private static OrderResponse MapToOrderResponse(IRecord record)
        {
            var properties = record["o"].As<INode>().Properties;

            var orderResponse = new OrderResponse
            {
                Id = properties["Id"].As<int>(),
                OrderDate = properties["OrderDate"].As<DateTimeOffset>()
            };

            return orderResponse;
        }

        public async Task<OrderResponse> AddOrderWithDetails(OrderRequest orderToAdd)
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

        public async Task<OrderResponse> GetOrder(int id)
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveProduct = "MATCH (o:Order) WHERE o.Id=$id RETURN o";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveProduct, new { id });
                        return await queryResult.SingleAsync();
                    }
                    catch
                    {
                        throw new NotFoundOrderException(id);
                    }
                });

                var orderResponse = MapToOrderResponse(result);

                return orderResponse;
            }
        }

        public async Task<List<OrderResponse>> GetOrders()
        {
            using (var session = _driver.AsyncSession())
            {
                var retrieveNodesCypher = "MATCH (o:Order) RETURN o";
                var result = await session.ReadTransactionAsync(async transaction =>
                {
                    try
                    {
                        var queryResult = await transaction.RunAsync(retrieveNodesCypher);
                        return await queryResult.ToListAsync();
                    }
                    catch
                    {
                        throw new NotFoundOrderException();
                    }
                });

                var ordersResponse = result.Select(record => MapToOrderResponse(record)).ToList();

                return ordersResponse;
            }
        }
    }
}
