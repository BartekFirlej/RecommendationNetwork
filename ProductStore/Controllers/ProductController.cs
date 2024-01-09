using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        public ProductController(IProductService productService, RabbitMqPublisher rabbitMqPublisher)
        {
            _productService = productService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                return Ok(products);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductResponse(id);
                return Ok(product);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductRequest productToAdd)
        {
            try
            {
                var addedProduct = await _productService.PostProduct(productToAdd);
                _rabbitMqPublisher.PublishMessage(addedProduct, "productQueue");
                return CreatedAtAction(nameof(PostProduct), addedProduct);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("api")]
        public async Task<IActionResult> PostProduct()
        {
            try
            {
                var addedProduct = await _productService.PostProductFromAPI();
                _rabbitMqPublisher.PublishMessage(addedProduct, "productQueue");
                return CreatedAtAction(nameof(PostProduct), addedProduct);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deletedProduct = await _productService.DeleteProduct(id);
                return Ok(deletedProduct);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}