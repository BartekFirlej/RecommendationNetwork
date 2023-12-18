using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;

namespace RecommendationNetwork.Services
{
    public interface IOrderService
    {
        public Task<OrderResponse> AddOrder(OrderRequest orderToAdd);
        public Task<List<OrderResponse>> GetOrders();
        public Task<OrderResponse> GetOrder(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> AddOrder(OrderRequest orderToAdd)
        {
            return await _orderRepository.AddOrder(orderToAdd);
        }

        public async Task<OrderResponse> GetOrder(int id)
        {
            return await _orderRepository.GetOrder(id);
        }

        public async Task<List<OrderResponse>> GetOrders()
        {
            return await _orderRepository.GetOrders();
        }
    }
}
