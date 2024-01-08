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
    public async Task<IActionResult> GetCustomersCustomersRecommenadtions()
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
    public async Task<IActionResult> GetCustomerCustomerRecommendations(int id)
    {
        try
        {
            var customerRecommendations = await _customerRecommendationService.GetCustomersCustomerRecommmendation(id);
            return Ok(customerRecommendations);
        }
        catch(NotFoundCustomerException e)
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
}
