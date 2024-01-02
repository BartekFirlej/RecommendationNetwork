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
        private readonly IPurchaseDetailService _purchaseDetailService;
        public PurchaseController(IPurchaseService purchaseService, IPurchaseDetailService purchaseDetailService)
        {
            _purchaseService = purchaseService;
            _purchaseDetailService = purchaseDetailService;
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

        [HttpPost("details")]
        public async Task<IActionResult> PostPurchaseWithDetails(PurchaseWithDetailsRequest purchaseToAdd)
        {
            try
            {
                var addedPurchase = await _purchaseService.PostPurchaseWithDetails(purchaseToAdd);
                return CreatedAtAction(nameof(PostPurchaseWithDetails), addedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("detail")]
        public async Task<IActionResult> PostPurchaseDetail(PurchaseIdDetailRequest purchaseToAdd)
        {
            try
            {
                var addedPurchase = await _purchaseDetailService.AddPurchaseDetail(purchaseToAdd);
                return CreatedAtAction(nameof(PostPurchase), addedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{id}/detail")]
        public async Task<IActionResult> PostPurchaseDetail(PurchaseDetailRequest purchaseToAdd, int id)
        {
            try
            {
                var addedPurchase = await _purchaseDetailService.AddPurchaseDetail(purchaseToAdd, id);
                return CreatedAtAction(nameof(PostPurchase), addedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
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