using Microsoft.AspNetCore.Mvc;
using RecommendationNetwork.DTOs;
using RecommendationNetwork.Services;

[ApiController]
[Route("voivodeship")]
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
        var addedVoivodeship = await _voivodeshipService.AddVoivodeship(voivodeshipToAdd);
        return Ok(addedVoivodeship);
    }

    [HttpGet]
    public async Task<IActionResult> GetVoivodeships()
    {
        var voivodeships = await _voivodeshipService.GetVoivodeships();
        return Ok(voivodeships);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVoivodeship(int id)
    {
        var voivodeships = await _voivodeshipService.GetVoivodeship(id);
        return Ok(voivodeships);
    }
}

