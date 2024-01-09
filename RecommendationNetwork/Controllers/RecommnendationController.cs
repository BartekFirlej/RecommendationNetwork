using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

[ApiController]
[Route("recommmendations")]
public class RecommmendationController : ControllerBase
{
    private readonly IPurchaseRecommendationService _purchaseRecommendationService;
    private readonly ICustomerRecommendationService _customerRecommendationService;
    public RecommmendationController(IPurchaseRecommendationService purchaseRecommendationService, ICustomerRecommendationService customerRecommendationService)
    {
        _purchaseRecommendationService = purchaseRecommendationService;
        _customerRecommendationService = customerRecommendationService;
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomersCustomersRecommendations()
    {
        try
        {
            var customerRecommendations = await _customerRecommendationService.GetCustomersCustomersRecommmendations();
            return Ok(customerRecommendations);
        }
        catch (NotFoundCustomerRecommendationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomersCustomerRecommendations(int id)
    {
        try
        {
            var customerRecommendations = await _customerRecommendationService.GetCustomersCustomerRecommmendations(id);
            return Ok(customerRecommendations);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (NotFoundCustomerRecommendationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("purchases")]
    public async Task<IActionResult> GetPurchasesCustomersRecommenadations()
    {
        try
        {
            var customerRecommendations = await _purchaseRecommendationService.GetPurchasesCustomersRecommmendations();
            return Ok(customerRecommendations);
        }
        catch (NotFoundPurchasesRecommendationsException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("purchases/{id}")]
    public async Task<IActionResult> GetPurchasesCustomerRecommendations(int id)
    {
        try
        {
            var customerRecommendations = await _purchaseRecommendationService.GetPurchasesCustomerRecommmendations(id);
            return Ok(customerRecommendations);
        }
        catch (NotFoundCustomerException e)
        {
            return NotFound(e.Message);
        }
        catch (NotFoundPurchasesRecommendationsException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}