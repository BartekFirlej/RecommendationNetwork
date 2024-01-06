using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Exceptions;
using RecommendationNetwork.Services;

namespace RecommendationNetwork.Controllers
{
    [ApiController]
    [Route("purchase-proposals")]
    public class PurchaseProposalController : ControllerBase
    {
        private readonly IPurchaseProposalService _purchaseProposalService;
        public PurchaseProposalController(IPurchaseProposalService purchaseProposalService)
        {
            _purchaseProposalService = purchaseProposalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseProposal(PurchaseProposalRequest purchaseProposalRequest)
        {
            try
            {
                var createdPurchaseProposal = await _purchaseProposalService.AddPurchaseProposal(purchaseProposalRequest);
                return Ok(createdPurchaseProposal);
            }
            catch (NotFoundProductException e)
            {
                return NotFound(e.Message);
            }
            catch (NotFoundCustomerException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseProposals()
        {
            try
            {
                var purchaseProposals = await _purchaseProposalService.GetPurchaseProposals();
                return Ok(purchaseProposals);
            }
            catch (NotFoundPurchaseProposalException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{customerid}/{productid}")]
        public async Task<IActionResult> GetPurchaseProposal(int customerid, int productid)
        {
            try
            {
                var purchaseProposal = await _purchaseProposalService.GetPurchaseProposal(customerid, productid);
                return Ok(purchaseProposal);
            }
            catch (NotFoundPurchaseProposalException e)
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
