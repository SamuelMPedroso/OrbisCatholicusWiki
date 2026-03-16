using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;

namespace OrbisCatholicus.API.Controllers;

[ApiController]
[Route("api")]
public class ArticleVersionsController : ControllerBase
{
    private readonly IArticleVersionService _versionService;

    public ArticleVersionsController(IArticleVersionService versionService)
    {
        _versionService = versionService;
    }

    [HttpGet("articles/{articleId:int}/versions")]
    public async Task<IActionResult> GetVersions(int articleId)
    {
        var result = await _versionService.GetByArticleIdAsync(articleId);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [Authorize(Roles = "Autor,Revisor,Administrador")]
    [HttpPost("articles/{articleId:int}/versions")]
    public async Task<IActionResult> CreateVersion(int articleId, [FromBody] CreateVersionDto dto)
    {
        var userId = GetUserId();
        var result = await _versionService.CreateVersionAsync(articleId, dto, userId);
        return result.Success ? Created(string.Empty, result.Data) : BadRequest(result.Error);
    }

    [Authorize(Roles = "Revisor,Administrador")]
    [HttpPut("versions/{versionId:int}/review")]
    public async Task<IActionResult> Review(int versionId, [FromBody] ReviewVersionDto dto)
    {
        var reviewerId = GetUserId();
        var result = await _versionService.ReviewAsync(versionId, dto, reviewerId);
        return result.Success ? NoContent() : BadRequest(result.Error);
    }

    [Authorize(Roles = "Revisor,Administrador")]
    [HttpGet("versions/pending")]
    public async Task<IActionResult> GetPendingReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _versionService.GetPendingReviewsAsync(page, pageSize);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
