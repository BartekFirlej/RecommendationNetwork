using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Route("purchases")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            try
            {
                var purchases = await _purchaseService.GetPurchases();
                return Ok(purchases);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchase(int id)
        {
            try
            {
                var purchase = await _purchaseService.GetPurchaseResponse(id);
                return Ok(purchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPurchase(PurchaseRequest purchaseToAdd)
        {
            try
            {
                var addedPurchase = await _purchaseService.PostPurchase(purchaseToAdd);
                return CreatedAtAction(nameof(PostPurchase), addedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeltePurchase(int id)
        {
            try
            {
                var deletedPurchase = await _purchaseService.DeletePurchase(id);
                return Ok(deletedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
