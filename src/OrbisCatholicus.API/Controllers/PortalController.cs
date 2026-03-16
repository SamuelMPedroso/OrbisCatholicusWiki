using Microsoft.AspNetCore.Mvc;
using OrbisCatholicus.Application.Interfaces;

namespace OrbisCatholicus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortalController : ControllerBase
{
    private readonly IPortalService _portalService;

    public PortalController(IPortalService portalService)
    {
        _portalService = portalService;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _portalService.GetStatsAsync();
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }
}
