using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;

[ApiController]
[Route("users")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerRequest customerToAdd)
    {
        var createdNode = await _customerService.AddCustomer(customerToAdd);
        return Ok(createdNode);
    }

    [HttpGet]
    public async Task<IActionResult> ReadCustomers()
    {
        var customerNodes = await _customerService.GetCustomers();
        return Ok(customerNodes);
    }
}
