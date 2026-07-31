using LogisticsPackingService.Application.DTOs;
using LogisticsPackingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPackingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PackingController : ControllerBase
{
    private readonly IPackingService _packingService;

    public PackingController(IPackingService packingService)
    {
        _packingService = packingService;
    }

    [HttpPost("calculate")]
    [ProducesResponseType(typeof(PackingResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<PackingResponseDto> Calculate(
        [FromBody] PackingRequestDto request)
    {
        var response = _packingService.CalculateBoxes(request);

        return Ok(response);
    }
}