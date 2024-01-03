using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Route("purchase-details")]
    [ApiController]
    public class PurchaseDetailController : ControllerBase
    {
        private readonly IPurchaseDetailService _purchaseDetailService;
        public PurchaseDetailController(IPurchaseDetailService purchaseDetailService)
        {
            _purchaseDetailService = purchaseDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseDetailss()
        {
            try
            {
                var purchaseDetails = await _purchaseDetailService.GetPurchaseDetails();
                return Ok(purchaseDetails);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseDetail(int id)
        {
            try
            {
                var purchaseDetail = await _purchaseDetailService.GetPurchaseDetailResponse(id);
                return Ok(purchaseDetail);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPurchaseDetail(PurchaseIdDetailRequest purchaseToAdd)
        {
            try
            {
                var addedPurchase = await _purchaseDetailService.AddPurchaseDetail(purchaseToAdd);
                return CreatedAtAction(nameof(PostPurchaseDetail), addedPurchase);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{orderid}")]
        public async Task<IActionResult> PostPurchaseDetail(PurchaseDetailRequest purchaseToAdd, int orderid)
        {
            try
            {
                var addedPurchaseDetail = await _purchaseDetailService.AddPurchaseDetail(purchaseToAdd, orderid);
                return CreatedAtAction(nameof(PostPurchaseDetail), addedPurchaseDetail);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseDetail(int id)
        {
            try
            {
                var deletedPurchaseDetail = await _purchaseDetailService.DeletePurchaseDetail(id);
                return Ok(deletedPurchaseDetail);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}