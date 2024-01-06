using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

[ApiController]
[Route("purchases")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;
    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchase(PurchaseRequest orderToAdd)
    {
        try
        {
            var createdNode = await _purchaseService.AddPurchase(orderToAdd);
            return Ok(createdNode);
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

    [HttpPost("details")]
    public async Task<IActionResult> CreatePurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd)
    {
        try
        {
            var createdNode = await _purchaseService.AddPurchaseWithDetails(purchaseToAdd);
            return Ok(createdNode);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (NotFoundProductException e)
        {
            return NotFound(e.Message);
        }
        catch (ValueMustBeGreaterThanZeroException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPurchases()
    {
        try
        {
            var orderNodes = await _purchaseService.GetPurchases();
            return Ok(orderNodes);
        }
        catch (NotFoundPurchaseException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchase(int id)
    {
        try
        {
            var order = await _purchaseService.GetPurchase(id);
            return Ok(order);
        }
        catch (NotFoundPurchaseException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

