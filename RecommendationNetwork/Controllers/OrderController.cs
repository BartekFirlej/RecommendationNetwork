using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderRequest orderToAdd)
    {
        var createdNode = await _orderService.AddOrder(orderToAdd);
        return Ok(createdNode);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orderNodes = await _orderService.GetOrders();
        return Ok(orderNodes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _orderService.GetOrder(id);
        return Ok(order);
    }
}

