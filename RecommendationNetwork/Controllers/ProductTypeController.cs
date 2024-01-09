using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

[ApiController]
[Route("product-types")]
public class ProductTypeController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypeController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductType(ProductTypeRequest productTypeToAdd)
    {
        try
        {
            var addedProductType = await _productTypeService.AddProductType(productTypeToAdd);
            return Ok(addedProductType);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProductTypes()
    {
        try
        {
            var productTypes = await _productTypeService.GetProductTypes();
            return Ok(productTypes);
        }
        catch (NotFoundProductTypeException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductType(int id)
    {
        try
        {
            var productType = await _productTypeService.GetProductType(id);
            return Ok(productType);
        }
        catch (NotFoundProductTypeException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}