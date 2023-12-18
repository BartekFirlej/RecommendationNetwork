using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
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
        var addedProductType = await _productTypeService.AddProductType(productTypeToAdd);
        return Ok(addedProductType);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductTypes()
    {
        var productTypes = await _productTypeService.GetProductTypes();
        return Ok(productTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductType(int id)
    {
        var productType = await _productTypeService.GetProductType(id);
        return Ok(productType);
    }
}

