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

        [HttpGet("details")]
        public async Task<IActionResult> GetPurchasesWithDetails()
        {
            try
            {
                var purchases = await _purchaseService.GetPurchasesWithDetails();
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

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetCustomersPurchases(int id)
        {
            try
            {
                var purchase = await _purchaseService.GetCustomersPurchases(id);
                return Ok(purchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetPurchaseWithDetails(int id)
        {
            try
            {
                var purchase = await _purchaseService.GetPurchaseWithDetails(id);
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