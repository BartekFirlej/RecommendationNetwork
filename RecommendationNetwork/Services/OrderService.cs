using RecommendationNetwork.DTOs;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Exceptions;

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
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        public OrderService(IOrderRepository orderRepository, ICustomerService customerService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<OrderResponse> AddOrder(OrderRequest orderToAdd)
        {
            await _customerService.GetCustomer(orderToAdd.CustomerId);
            if(orderToAdd.RecommenderId!=null)
            {
                await _customerService.GetCustomer((int)orderToAdd.RecommenderId);
            }
            foreach(var product in orderToAdd.OrderDetails)
            {
                await _productService.GetProduct(product.Id);
                if(product.Quantity <= 0)
                {
                    throw new QuantityMustBeGreaterThanZeroException(product.Id);
                }
            }
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
