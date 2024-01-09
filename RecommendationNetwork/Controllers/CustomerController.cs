using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

[ApiController]
[Route("customers")]
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
        try
        {
            var createdNode = await _customerService.AddCustomer(customerToAdd);
            return Ok(createdNode);
        }
        catch (NotFoundVoivodeshipException e)
        {
            return NotFound(e.Message);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ReadCustomers()
    {
        try
        {
            var customerNodes = await _customerService.GetCustomers();
            return Ok(customerNodes);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomer(id);
            return Ok(customer);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}