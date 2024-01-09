using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Route("product-types")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;

        public ProductTypeController(IProductTypeService productTypeService, RabbitMqPublisher rabbitMqPublisher)
        {
            _productTypeService = productTypeService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductTypes()
        {
            try
            {
                var productTypes = await _productTypeService.GetProductTypes();
                return Ok(productTypes);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductType(int id)
        {
            try
            {
                var productType = await _productTypeService.GetProductTypeResponse(id);
                return Ok(productType);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProductType(ProductTypeRequest productTypeToAdd)
        {
            try
            {
                var addedProductType = await _productTypeService.PostProductType(productTypeToAdd);
                _rabbitMqPublisher.PublishMessage(addedProductType, "productTypeQueue");
                return CreatedAtAction(nameof(PostProductType), addedProductType);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                var deletedProductType = await _productTypeService.DeleteProductType(id);
                return Ok(deletedProductType);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}