using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductRequest productToAdd)
    {
        try
        {
            var addedProduct = await _productService.AddProduct(productToAdd);
            return Ok(addedProduct);
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

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }
        catch (NotFoundProductException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProduct(id);
            return Ok(product);
        }
        catch (NotFoundProductException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}