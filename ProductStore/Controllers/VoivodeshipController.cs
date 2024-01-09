using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductStore.DTOs;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [ApiController]
    [Route("voivodeships")]
    public class VoivodeshipController : ControllerBase
    {
        private readonly IVoivodeshipService _voivodeshipService;

        public VoivodeshipController(IVoivodeshipService voivodeshipService)
        {
            _voivodeshipService = voivodeshipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVoivodeships()
        {
            ICollection<VoivodeshipResponse> voivodeships;
            try
            {
                voivodeships = await _voivodeshipService.GetVoivodeships();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(voivodeships);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoivodeship(int id)
        {
            VoivodeshipResponse voivodeship;
            try
            {
                voivodeship = await _voivodeshipService.GetVoivodeshipResponse(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(voivodeship);
        }

        [HttpPost]
        public async Task<IActionResult> PostVoivodeship(VoivodeshipRequest voiovdeshipToAdd)
        {
            VoivodeshipResponse voivodeship;
            try
            {
                voivodeship = await _voivodeshipService.PostVoivodeship(voiovdeshipToAdd);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(voivodeship);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoivodeship(int id)
        {
            VoivodeshipResponse voivodeship;
            try
            {
                voivodeship = await _voivodeshipService.DeleteVoivodeship(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(voivodeship);
        }
    }
}