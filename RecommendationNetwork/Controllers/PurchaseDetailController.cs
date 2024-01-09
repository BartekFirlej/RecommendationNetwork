using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

namespace RecommendationNetwork.Controllers
{
    [ApiController]
    [Route("purchase-details")]
    public class PurchaseDetailController : ControllerBase
    {
        private readonly IPurchaseDetailService _purchaseDetailService;
        public PurchaseDetailController(IPurchaseDetailService purchaseDetailService)
        {
            _purchaseDetailService = purchaseDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseDetail(PurchaseIdDetailRequest purchaseDetailToAdd)
        {
            try
            {
                var createdPurchaseDetail = await _purchaseDetailService.AddPurchaseDetail(purchaseDetailToAdd);
                return Ok(createdPurchaseDetail);
            }
            catch (NotFoundProductException e)
            {
                return NotFound(e.Message);
            }
            catch (ValueMustBeGreaterThanZeroException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundPurchaseException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseDetails()
        {
            try
            {
                var purchaseDetails = await _purchaseDetailService.GetPurchaseDetails();
                return Ok(purchaseDetails);
            }
            catch (NotFoundPurchaseDetailException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseDetail(int id)
        {
            try
            {
                var purchaseDetail = await _purchaseDetailService.GetPurchaseDetail(id);
                return Ok(purchaseDetail);
            }
            catch (NotFoundPurchaseDetailException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}