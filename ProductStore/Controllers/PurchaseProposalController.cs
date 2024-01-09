using ProductStore.Services;
using Microsoft.AspNetCore.Mvc;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Route("purchase-proposals")]
    [ApiController]
    public class PurchaseProposalController : ControllerBase
    {
        private readonly IPurchaseProposalService _purchaseProposalService;

        public PurchaseProposalController(IPurchaseProposalService purchaseProposalService)
        {
            _purchaseProposalService = purchaseProposalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseProposals()
        {
            try
            {
                var purchaseProposals = await _purchaseProposalService.GetPurchaseProposals();
                return Ok(purchaseProposals);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseProposal(int id)
        {
            try
            {
                var purchaseProposal = await _purchaseProposalService.GetPurchaseProposalResponse(id);
                return Ok(purchaseProposal);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPurchaseProposal(PurchaseProposalRequest purchaseProposalToAdd)
        {
            try
            {
                var addedPurchaseProposal = await _purchaseProposalService.PostPurchaseProposal(purchaseProposalToAdd);
                return CreatedAtAction(nameof(PostPurchaseProposal), addedPurchaseProposal);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseProposal(int id)
        {
            try
            {
                var deletedPurchaseProposal = await _purchaseProposalService.DeletePurchaseProposal(id);
                return Ok(deletedPurchaseProposal);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

    }
}