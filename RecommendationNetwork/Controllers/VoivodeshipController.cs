using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;
using RecommendationNetwork.Exceptions;

[ApiController]
[Route("voivodeships")]
public class VoivodeshipController : ControllerBase
{
    private readonly IVoivodeshipService _voivodeshipService;
    public VoivodeshipController(IVoivodeshipService voivodeshipService)
    {
        _voivodeshipService = voivodeshipService;
    }

    [HttpPost]
    public async Task<IActionResult> AddVoivodeship(VoivodeshipRequest voivodeshipToAdd)
    {
        try
        {
            var addedVoivodeship = await _voivodeshipService.AddVoivodeship(voivodeshipToAdd);
            return CreatedAtAction(nameof(AddVoivodeship), addedVoivodeship);
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetVoivodeships()
    {
        try
        {
            var voivodeships = await _voivodeshipService.GetVoivodeships();
            return Ok(voivodeships);
        }
        catch(NotFoundVoivodeshipException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVoivodeship(int id)
    {
        try
        {
            var voivodeships = await _voivodeshipService.GetVoivodeship(id);
            return Ok(voivodeships);
        }
        catch (NotFoundVoivodeshipException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

